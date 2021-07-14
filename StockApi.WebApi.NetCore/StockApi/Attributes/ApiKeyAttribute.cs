namespace StockApi.Attributes
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///   Handle api key validation of request.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ApiKeyAttribute : Attribute, IAsyncActionFilter
	{
		/// <summary>
		///   Name of configuration entry that stores a valid api key.
		/// </summary>
		private const string ApiKeyConfigName = "ApiKey";

		/// <summary>
		///   Name of the header entry for the api key.
		/// </summary>
		private const string ApiKeyName = "x-api-key";

		/// <summary>
		///   Validate api keys.
		/// </summary>
		/// <param name="context">The current <see cref="ActionExecutingContext" />.</param>
		/// <param name="next">The next function.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
			var validApiKey = configuration[ApiKeyConfigName];

			if (context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var apiKey)
			    && apiKey == validApiKey)
			{
				await next();
			}
			else
			{
				context.Result = new UnauthorizedResult();
			}
		}
	}
}