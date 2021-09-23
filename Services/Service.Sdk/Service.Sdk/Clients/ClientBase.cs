namespace Service.Sdk.Clients
{
	using System;
	using System.IO;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using Service.Contracts.Business.Log;
	using Service.Contracts.Client;

	/// <summary>
	///   Base class for client implementations.
	/// </summary>
	public abstract class ClientBase
	{
		/// <summary>
		///   The api key for the service call.
		/// </summary>
		private readonly string apiKey;

		/// <summary>
		///   Logger for error messages.
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		///   Creates a new instance of <see cref="ClientBase" />.
		/// </summary>
		/// <param name="logger">Logger for error messages.</param>
		protected ClientBase(ILogger logger)
			: this(logger, null)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		///   Creates a new instance of <see cref="ClientBase" />.
		/// </summary>
		/// <param name="logger">Logger for error messages.</param>
		/// <param name="apiKey">The api key for service calls.</param>
		protected ClientBase(ILogger logger, string apiKey)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.apiKey = apiKey;
		}

		/// <summary>
		///   Calls a service.
		/// </summary>
		/// <param name="requestUrl">The url of the service.</param>
		/// <param name="httpMethod">The http method of the operation.</param>
		/// <returns>A <see cref="Task" /> whose result is an <see cref="IEmptyClientResult" />.</returns>
		protected async Task<IEmptyClientResult> ServiceCall(
			string requestUrl,
			string httpMethod)
		{
			try
			{
				var request = this.Create(requestUrl, httpMethod);
				using var response = await request.GetResponseAsync() as HttpWebResponse;
				var clientResult = CreateEmptyClientResult(response);
				return clientResult;
			}
			catch (Exception ex)
			{
				return await this.HandleException(ex, requestUrl, httpMethod);
			}
		}

		/// <summary>
		///   Calls a service.
		/// </summary>
		/// <typeparam name="TRequestData">The type of the request data.</typeparam>
		/// <param name="requestUrl">The url of the service.</param>
		/// <param name="httpMethod">The http method of the operation.</param>
		/// <param name="requestData">The data added to the body of the request.</param>
		/// <returns>A <see cref="Task" /> whose result is an <see cref="IEmptyClientResult" />.</returns>
		protected async Task<IEmptyClientResult> ServiceCall<TRequestData>(
			string requestUrl,
			string httpMethod,
			TRequestData requestData) where TRequestData : class
		{
			try
			{
				var request = this.Create(requestUrl, httpMethod);
				request = await AddRequestData(request, requestData);
				using var response = await request.GetResponseAsync() as HttpWebResponse;
				var clientResult = CreateEmptyClientResult(response);
				return clientResult;
			}
			catch (Exception ex)
			{
				return await this.HandleException(ex, requestUrl, httpMethod);
			}
		}

		/// <summary>
		///   Calls a service.
		/// </summary>
		/// <typeparam name="TResponseData">The data of the response.</typeparam>
		/// <param name="requestUrl">The url of the service.</param>
		/// <param name="httpMethod">The http method of the operation.</param>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="IClientResult{TResponseData}" />.</returns>
		protected async Task<IClientResult<TResponseData>> ServiceCallWithResult<TResponseData>(
			string requestUrl,
			string httpMethod) where TResponseData : class
		{
			try
			{
				var request = this.Create(requestUrl, httpMethod);
				using var response = await request.GetResponseAsync() as HttpWebResponse;
				return await CreateClientResult<TResponseData>(response);
			}
			catch (Exception ex)
			{
				return await this.HandleExceptionWithResult<TResponseData>(ex, requestUrl, httpMethod);
			}
		}

		/// <summary>
		///   Calls a service.
		/// </summary>
		/// <typeparam name="TRequestData">The type of the request data.</typeparam>
		/// <typeparam name="TResponseData">The type of the response data.</typeparam>
		/// <param name="requestUrl">The url of the service.</param>
		/// <param name="httpMethod">The http method of the operation.</param>
		/// <param name="requestData">The data of the request.</param>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="IClientResult{TResponseData}" />.</returns>
		protected async Task<IClientResult<TResponseData>> ServiceCallWithResult<TRequestData, TResponseData>(
			string requestUrl,
			string httpMethod,
			TRequestData requestData)
			where TResponseData : class
			where TRequestData : class
		{
			try
			{
				var request = this.Create(requestUrl, httpMethod);
				request = await AddRequestData(request, requestData);
				using var response = await request.GetResponseAsync() as HttpWebResponse;
				return await CreateClientResult<TResponseData>(response);
			}
			catch (Exception ex)
			{
				return await this.HandleExceptionWithResult<TResponseData>(ex, requestUrl, httpMethod);
			}
		}

		/// <summary>
		///   Add the request data to the body of the request.
		/// </summary>
		/// <typeparam name="TRequestData">The type of the request data.</typeparam>
		/// <param name="request">The <paramref name="requestData" /> is added to this request.</param>
		/// <param name="requestData">The data that is added to the body of the request.</param>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="HttpWebRequest" />.</returns>
		private static async Task<HttpWebRequest> AddRequestData<TRequestData>(
			HttpWebRequest request,
			TRequestData requestData)
			where TRequestData : class
		{
			if (requestData != null)
			{
				var json = JsonConvert.SerializeObject(requestData);
				var data = Encoding.UTF8.GetBytes(json);
				request.ContentType = "application/json";
				request.ContentLength = data.Length;
				await using var requestStream = request.GetRequestStream();
				await requestStream.WriteAsync(data, 0, data.Length);
			}

			return request;
		}

		/// <summary>
		///   Creates a new <see cref="HttpWebRequest" />.
		/// </summary>
		/// <param name="requestUrl">The url of the service.</param>
		/// <param name="httpMethod">The http method of the operation.</param>
		/// <returns>A new <see cref="HttpWebRequest" />.</returns>
		private HttpWebRequest Create(
			string requestUrl,
			string httpMethod)
		{
			var request = WebRequest.CreateHttp(requestUrl);
			request.Method = httpMethod;
			if (!string.IsNullOrWhiteSpace(this.apiKey))
			{
				request.Headers.Add("x-api-key", this.apiKey);
			}

			return request;
		}

		/// <summary>
		///   Creates a new <see cref="ClientResult{TResponseData}" /> from the given <paramref name="response" />.
		/// </summary>
		/// <typeparam name="TResponseData">The type of the response data.</typeparam>
		/// <param name="response">The response from that the <see cref="ClientResult{TResponseData}" /> is created.</param>
		/// <returns>A new <see cref="ClientResult{TResponseData}" />.</returns>
		private static async Task<IClientResult<TResponseData>> CreateClientResult<TResponseData>(HttpWebResponse response)
		{
			if (response == null)
			{
				return new ClientResult<TResponseData>
				{
					StatusCode = HttpStatusCode.InternalServerError
				};
			}

			var clientResult = new ClientResult<TResponseData>
			{
				StatusCode = response.StatusCode
			};

			await using var stream = response.GetResponseStream();
			if (stream != null)
			{
				using var reader = new StreamReader(stream);
				var result = await reader.ReadToEndAsync();
				var resultData = JsonConvert.DeserializeObject<TResponseData>(result);
				clientResult.ResponseData = resultData;
			}

			return clientResult;
		}

		/// <summary>
		///   Creates a new <see cref="EmptyClientResult" /> from the given <see cref="HttpWebResponse" />.
		/// </summary>
		/// <param name="response">A <see cref="HttpWebResponse" />.</param>
		/// <returns>A <see cref="EmptyClientResult" />.</returns>
		private static EmptyClientResult CreateEmptyClientResult(HttpWebResponse response)
		{
			if (response == null)
			{
				return new EmptyClientResult
				{
					StatusCode = HttpStatusCode.InternalServerError
				};
			}

			return new EmptyClientResult
			{
				StatusCode = response.StatusCode
			};
		}

		/// <summary>
		///   Handles the given <see cref="Exception" /> and creates an <see cref="EmptyClientResult" />.
		/// </summary>
		/// <param name="ex">The thrown <see cref="Exception" />.</param>
		/// <param name="requestUrl">The url of the service.</param>
		/// <param name="httpMethod">The http method of the operation.</param>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="EmptyClientResult" />.</returns>
		private async Task<EmptyClientResult> HandleException(Exception ex, string requestUrl, string httpMethod)
		{
			await this.LogError(ex, $"Error while calling {httpMethod} {requestUrl}");
			if (ex is WebException
			{
				Response: HttpWebResponse webResponse
			})
			{
				return new EmptyClientResult
				{
					Exception = ex,
					StatusCode = webResponse.StatusCode
				};
			}

			return new EmptyClientResult
			{
				Exception = ex,
				StatusCode = HttpStatusCode.InternalServerError
			};
		}

		/// <summary>
		///   Handles the given <see cref="Exception" /> and creates an <see cref="ClientResult{TResponseData}" />.
		/// </summary>
		/// <typeparam name="TResponseData">The type of the response data.</typeparam>
		/// <param name="ex">The thrown <see cref="Exception" />.</param>
		/// <param name="requestUrl">The url of the service.</param>
		/// <param name="httpMethod">The http method of the operation.</param>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClientResult{TResponseData}" />.</returns>
		private async Task<ClientResult<TResponseData>> HandleExceptionWithResult<TResponseData>(
			Exception ex,
			string requestUrl,
			string httpMethod)
		{
			await this.LogError(ex, $"Error while calling {httpMethod} {requestUrl}");
			if (ex is WebException
			{
				Response: HttpWebResponse webResponse
			})
			{
				return new ClientResult<TResponseData>
				{
					Exception = ex,
					StatusCode = webResponse.StatusCode
				};
			}

			return new ClientResult<TResponseData>
			{
				Exception = ex,
				StatusCode = HttpStatusCode.InternalServerError
			};
		}

		/// <summary>
		///   Logs an <see cref="Exception" /> including an error message.
		/// </summary>
		/// <param name="ex">The exception to be logged.</param>
		/// <param name="message">The error message.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task LogError(Exception ex, string message)
		{
			try
			{
				await this.logger.Error(message, ex);
			}
			catch
			{
				// unable to handle errors while logging
			}
		}
	}
}