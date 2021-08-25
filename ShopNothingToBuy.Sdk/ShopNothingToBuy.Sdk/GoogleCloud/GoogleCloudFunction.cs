namespace ShopNothingToBuy.Sdk.GoogleCloud
{
	using System;
	using System.IO;
	using System.Net;
	using System.Threading.Tasks;
	using Google.Cloud.Functions.Framework;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;

	/// <summary>
	///   Base class for google cloud functions.
	/// </summary>
	/// <typeparam name="TDelete">Type of a delete request body.</typeparam>
	/// <typeparam name="TGet">Type of a get request body.</typeparam>
	/// <typeparam name="TPost">Type of a post request body.</typeparam>
	/// <typeparam name="TPut">Type of a put request body.</typeparam>
	/// <typeparam name="TLogger">Type of the <see cref="ILogger{T}" />.</typeparam>
	public abstract class GoogleCloudFunction<TDelete, TGet, TPost, TPut, TLogger> : IHttpFunction
		where TDelete : class, new()
		where TGet : class, new()
		where TPost : class, new()
		where TPut : class, new()
	{
		/// <summary>
		///   Default name of the header that used for the api key.
		/// </summary>
		private const string DefaultApiKeyHeaderName = "x-api-key";

		/// <summary>
		///   Name of the header that used for the api key.
		/// </summary>
		private readonly string apiKeyHeaderName;

		/// <summary>
		///   A request header should contain this api key.
		/// </summary>
		private readonly string expectedApiKey;

		/// <summary>
		///   The error logger.
		/// </summary>
		private readonly ILogger<TLogger> logger;

		/// <summary>
		///   Creates a new instance of <see cref="GoogleCloudFunction{TDelete,TGet,TPost,TPut,TLogger}" />.
		/// </summary>
		/// <param name="expectedApiKey">A request header should contain this api key.</param>
		/// <param name="logger">The error logger.</param>
		protected GoogleCloudFunction(string expectedApiKey, ILogger<TLogger> logger)
			: this(expectedApiKey, DefaultApiKeyHeaderName, logger)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="GoogleCloudFunction{TDelete,TGet,TPost,TPut,TLogger}" />.
		/// </summary>
		/// <param name="expectedApiKey">A request header should contain this api key.</param>
		/// <param name="apiKeyHeaderName">Name of the request header containing the api key.</param>
		/// <param name="logger">The error logger.</param>
		protected GoogleCloudFunction(string expectedApiKey, string apiKeyHeaderName, ILogger<TLogger> logger)
		{
			if (expectedApiKey == null)
			{
				throw new ArgumentNullException(nameof(expectedApiKey));
			}

			if (apiKeyHeaderName == null)
			{
				throw new ArgumentNullException(nameof(apiKeyHeaderName));
			}

			if (string.IsNullOrWhiteSpace(expectedApiKey))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(expectedApiKey));
			}

			if (string.IsNullOrWhiteSpace(apiKeyHeaderName))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiKeyHeaderName));
			}

			this.expectedApiKey = expectedApiKey;
			this.apiKeyHeaderName = apiKeyHeaderName;
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		///   Execute the cloud function.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext" /> of the current execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task HandleAsync(HttpContext context)
		{
			try
			{
				if (!this.CheckApiKey(context))
				{
					return;
				}

				if (!await this.HandleMethod<TDelete>(HttpMethods.Delete, context, this.HandleDelete)
				    && !await this.HandleMethod<TGet>(HttpMethods.Get, context, this.HandleGet)
				    && !await this.HandleMethod<TPost>(HttpMethods.Post, context, this.HandlePost)
				    && !await this.HandleMethod<TPut>(HttpMethods.Put, context, this.HandlePut))
				{
					context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
				}
			}
			catch (Exception ex)
			{
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

				try
				{
					this.logger.LogError(ex, "unexpected error");
				}
				catch
				{
					// unable to log
				}
			}
		}

		/// <summary>
		///   Checks if the header contains the expected api key.
		/// </summary>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <returns>True if the expected api key matches the api key from the header of the request.</returns>
		protected virtual bool CheckApiKey(HttpContext context)
		{
			if (!context.Request.Headers.ContainsKey(this.apiKeyHeaderName)
			    || context.Request.Headers[this.apiKeyHeaderName] != this.expectedApiKey)
			{
				context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
				return false;
			}

			return true;
		}

		/// <summary>
		///   Handle delete requests.
		/// </summary>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <param name="body">The body of the request.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandleDelete(HttpContext context, TDelete body);

		/// <summary>
		///   Handle get requests.
		/// </summary>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <param name="body">The body of the request.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandleGet(HttpContext context, TGet body);

		/// <summary>
		///   Handle post requests.
		/// </summary>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <param name="body">The body of the request.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandlePost(HttpContext context, TPost body);

		/// <summary>
		///   Handle put requests.
		/// </summary>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <param name="body">The body of the request.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected abstract Task HandlePut(HttpContext context, TPut body);

		/// <summary>
		///   If <paramref name="method" /> matches <see cref="HttpRequest.Method" /> of the <paramref name="context" /> the
		///   <paramref name="handleHttpMethod" /> function will be called.
		/// </summary>
		/// <typeparam name="T">The type of the request body.</typeparam>
		/// <param name="method">The http method to be handled.</param>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <param name="handleHttpMethod">A function that handles the http method <paramref name="method" />.</param>
		/// <returns>True if <paramref name="method" /> and <see cref="HttpRequest.Method" /> matches and false otherwise.</returns>
		private async Task<bool> HandleMethod<T>(
			string method,
			HttpContext context,
			Func<HttpContext, T, Task> handleHttpMethod)
			where T : class, new()
		{
			if (string.Equals(context.Request.Method, method, StringComparison.CurrentCultureIgnoreCase))
			{
				var body = await ReadBody<T>(context);
				if (body != null)
				{
					await handleHttpMethod(context, body);
				}
				else
				{
					context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
				}

				return true;
			}

			return false;
		}

		/// <summary>
		///   Reads the body of the request.
		/// </summary>
		/// <typeparam name="T">The type of the body object.</typeparam>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <returns>An instance of <see cref="T" /> if body is valid and null otherwise.</returns>
		private static async Task<T> ReadBody<T>(HttpContext context) where T : class, new()
		{
			try
			{
				var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch
			{
				return typeof(T) == typeof(EmptyRequest) ? new T() : null;
			}
		}
	}
}