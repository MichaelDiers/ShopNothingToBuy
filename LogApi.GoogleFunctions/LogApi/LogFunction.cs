namespace LogApi
{
	using System;
	using System.IO;
	using System.Net;
	using System.Threading.Tasks;
	using Google.Cloud.Functions.Framework;
	using Google.Cloud.Functions.Hosting;
	using Microsoft.AspNetCore.Http;
	using MongoDB.Bson;
	using Newtonsoft.Json;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Extensions;
	using ShopNothingToBuy.Sdk.Models;

	/// <summary>
	///   An <see cref="IHttpFunction" /> using the google cloud.
	/// </summary>
	[FunctionsStartup(typeof(Startup))]
	public class LogFunction : IHttpFunction
	{
		// Todo: config
		private const string ApiKey = "0a1f9f6b-11ff-4ae6-9cb1-8efb33960d38";

		/// <summary>
		///   Name of the header that used for the api key.
		/// </summary>
		private const string ApiKeyName = "x-api-key";

		/// <summary>
		///   Service for accessing the database.
		/// </summary>
		private readonly IDatabase<LogEntry, ObjectId> database;

		/// <summary>
		///   Creates a new instance of <see cref="LogFunction" />.
		/// </summary>
		/// <param name="database">Service for accessing the database.</param>
		public LogFunction(IDatabase<LogEntry, ObjectId> database)
		{
			this.database = database ?? throw new ArgumentNullException(nameof(database));
		}

		/// <summary>
		///   The logic of the google cloud function.
		/// </summary>
		/// <param name="context">The context of the function.</param>
		/// <returns>An instance of <see cref="Task" />.</returns>
		public async Task HandleAsync(HttpContext context)
		{
			try
			{
				if (CheckApiKey(context, ApiKey) && HandleHttpMethods(context))
				{
					var logEntry = await ReadBody<LogEntryDto>(context);
					if (logEntry != null && logEntry.Level != LogLevel.None)
					{
						var result = await this.database.Create(new LogEntry(logEntry));
						context.Response.StatusCode = (int) result.ToHttpStatusCode();
					}
					else
					{
						context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
					}
				}
			}
			catch
			{
				// the logger cannot log its own errors
			}
		}


		/// <summary>
		///   Checks if the header contains the expected api key.
		/// </summary>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <param name="expectedApiKey">The api key that is expected.</param>
		/// <returns>True if the <paramref name="expectedApiKey" /> matches and false otherwise.</returns>
		private static bool CheckApiKey(HttpContext context, string expectedApiKey)
		{
			if (!context.Request.Headers.ContainsKey(ApiKeyName) || context.Request.Headers[ApiKeyName] != expectedApiKey)
			{
				context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
				return false;
			}

			return true;
		}

		/// <summary>
		///   Checks if the request method is <see cref="HttpMethods.Post" />.
		/// </summary>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <returns>True if the request is a post request and false otherwise.</returns>
		private static bool HandleHttpMethods(HttpContext context)
		{
			if (!string.Equals(context.Request.Method, HttpMethods.Post, StringComparison.CurrentCultureIgnoreCase))
			{
				context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
				return false;
			}

			return true;
		}

		/// <summary>
		///   Reads the body of the request.
		/// </summary>
		/// <typeparam name="T">The type of the body object.</typeparam>
		/// <param name="context">The current <see cref="HttpContext" />.</param>
		/// <returns>An instance of <see cref="T" /> if body is valid and null otherwise.</returns>
		private static async Task<T> ReadBody<T>(HttpContext context) where T : class
		{
			try
			{
				var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch
			{
				context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
				return null;
			}
		}
	}
}