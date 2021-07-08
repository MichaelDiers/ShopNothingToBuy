namespace ProductsApi
{
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Azure.WebJobs;
	using Microsoft.Azure.WebJobs.Extensions.Http;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;

	public static class Products
	{
		[FunctionName("GetProducts")]
		public static async Task<IActionResult> GetProducts(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
			ILogger log)
		{
			return new OkObjectResult(null);
		}
	}
}
