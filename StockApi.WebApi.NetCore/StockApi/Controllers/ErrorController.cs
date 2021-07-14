namespace StockApi.Controllers
{
	using System;
	using Microsoft.AspNetCore.Diagnostics;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json.Linq;
	using StockApi.Attributes;

	/// <summary>
	///   Handler for requests of unknown routes.
	/// </summary>
	[ApiController]
	[ApiKey]
	public class ErrorController : ControllerBase
	{
		/// <summary>
		///   Logger for errors.
		/// </summary>
		private readonly ILogger<ErrorController> logger;

		/// <summary>
		///   Creates a new instance of <see cref="ErrorController" />.
		/// </summary>
		/// <param name="logger">Error logger.</param>
		public ErrorController(ILogger<ErrorController> logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		///   Handle all exceptions.
		/// </summary>
		/// <returns></returns>
		[Route("error")]
		public IActionResult HandleException()
		{
			var context = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
			this.logger.LogError(context.Error, "Global error handling.");
			return new StatusCodeResult(500);
		}

		/// <summary>
		///   Send a <see cref="NotFoundObjectResult" /> for unknown routes.
		/// </summary>
		/// <returns>A <see cref="NotFoundObjectResult" />.</returns>
		[Route("{*url}", Order = 999)]
		public IActionResult HandleNotFound()
		{
			var question = new JObject {{"question", "Missed the intersection?"}};
			return new NotFoundObjectResult(question.ToString());
		}
	}
}