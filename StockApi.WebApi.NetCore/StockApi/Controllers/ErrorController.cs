namespace StockApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json.Linq;

	/// <summary>
	///   Handler for requests of unknown routes.
	/// </summary>
	[ApiController]
	public class ErrorController : ControllerBase
	{
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

		/// <summary>
		///   Handle all exceptions.
		/// </summary>
		/// <returns></returns>
		[Route("error")]
		public IActionResult HandleException()
		{
			// Todo: logging
			return this.Problem();
		}
	}
}