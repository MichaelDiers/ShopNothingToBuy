namespace Service.Sdk.GoogleCloud.Functions
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	///   Base class for google function implementations.
	/// </summary>
	public abstract class GoogleFunction
	{
		/// <summary>
		///   Name of the header that contains the api key.
		/// </summary>
		private const string HeaderApiKey = "x-api-key";

		/// <summary>
		///   Name of the header that contains the jwt.
		/// </summary>
		private const string HeaderAuthorization = "Authorization";

		/// <summary>
		///   Entry point of the google function.
		/// </summary>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task HandleAsync(HttpContext context)
		{
			try
			{
				var requestData = await this.ReadRequestData(context);
				await this.HandleMethod(requestData, context);
			}
			catch
			{
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
			}
		}

		/// <summary>
		///   Handle a http delete request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandleDelete(IRequestData requestData, HttpContext context);

		/// <summary>
		///   Handle a http get request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandleGet(IRequestData requestData, HttpContext context);

		/// <summary>
		///   Handle a http head request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandleHead(IRequestData requestData, HttpContext context);

		/// <summary>
		///   Handle a http post request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandlePost(IRequestData requestData, HttpContext context);

		/// <summary>
		///   Handle a http put request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandlePut(IRequestData requestData, HttpContext context);

		/// <summary>
		///   Determines the http method from the <paramref name="context" /> and class the matching handler.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task HandleMethod(IRequestData requestData, HttpContext context)
		{
			if (string.Equals(context.Request.Method, HttpMethods.Post, StringComparison.InvariantCultureIgnoreCase))
			{
				await this.HandlePost(requestData, context);
			}
			else if (string.Equals(context.Request.Method, HttpMethods.Get, StringComparison.InvariantCultureIgnoreCase))
			{
				await this.HandleGet(requestData, context);
			}
			else if (string.Equals(context.Request.Method, HttpMethods.Put, StringComparison.InvariantCultureIgnoreCase))
			{
				await this.HandlePut(requestData, context);
			}
			else if (string.Equals(context.Request.Method, HttpMethods.Delete, StringComparison.InvariantCultureIgnoreCase))
			{
				await this.HandleDelete(requestData, context);
			}
			else if (string.Equals(context.Request.Method, HttpMethods.Head, StringComparison.InvariantCultureIgnoreCase))
			{
				await this.HandleHead(requestData, context);
			}
			else
			{
				context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
			}
		}

		/// <summary>
		///   Reads the request data like body, header and query.
		/// </summary>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" /> whose result is an <see cref="IRequestData" />.</returns>
		private async Task<IRequestData> ReadRequestData(HttpContext context)
		{
			var requestData = new RequestData
			{
				Body = await new StreamReader(context.Request.Body).ReadToEndAsync()
			};

			if (context.Request.Query.Count != 0)
			{
				requestData.Query = new Dictionary<string, string>(
					context.Request.Query.Keys.Select(
						key => new KeyValuePair<string, string>(key.ToUpper(), context.Request.Query[key])));
			}

			if (context.Request.Headers.ContainsKey(HeaderApiKey))
			{
				requestData.ApiKey = context.Request.Headers[HeaderApiKey];
			}

			if (context.Request.Headers.ContainsKey(HeaderAuthorization))
			{
				string bearer = context.Request.Headers[HeaderAuthorization];
				if (!string.IsNullOrWhiteSpace(bearer))
				{
					var bearerSplit = bearer.Split(" ");
					if (bearerSplit.Length == 2)
					{
						requestData.Token = bearerSplit[1];
					}
				}
			}

			return requestData;
		}
	}
}