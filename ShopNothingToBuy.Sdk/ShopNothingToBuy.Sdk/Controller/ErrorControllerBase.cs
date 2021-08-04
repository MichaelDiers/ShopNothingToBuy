namespace ShopNothingToBuy.Sdk.Controller
{
	using System;
	using Microsoft.AspNetCore.Diagnostics;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///   Handler for requests of unknown routes.
	/// </summary>
	[ApiController]
	public class ErrorControllerBase : ControllerBase
	{
		/// <summary>
		///   Logger for errors.
		/// </summary>
		private readonly ILogger<ErrorControllerBase> logger;

		/// <summary>
		///   Creates a new instance of <see cref="ErrorControllerBase" />.
		/// </summary>
		/// <param name="logger">Error logger.</param>
		public ErrorControllerBase(ILogger<ErrorControllerBase> logger)
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
			return new NotFoundObjectResult(
				new
				{
					question = "Missed the intersection?"
				});
		}
	}
}