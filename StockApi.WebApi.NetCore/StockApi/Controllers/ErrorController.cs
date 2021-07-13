namespace StockApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json.Linq;

	/// <summary>
	///   Handler for requests of unknown routes.
	/// </summary>
	[Route("{*url}", Order = 999)]
	[ApiController]
	public class ErrorController : ControllerBase
	{
		/// <summary>
		///   Send a <see cref="NotFoundObjectResult" /> for unknown routes.
		/// </summary>
		/// <returns>A <see cref="NotFoundObjectResult" />.</returns>
		[HttpDelete]
		[HttpGet]
		[HttpPost]
		[HttpPut]
		public IActionResult HandleError()
		{
			var question = new JObject {{"question", "Missed the intersection?"}};
			return new NotFoundObjectResult(question.ToString());
		}
	}
}