namespace ProductsApi
{
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Azure.WebJobs;
	using Microsoft.Azure.WebJobs.Extensions.Http;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using ProductsApi.Contracts;

	public class Products
	{
		private readonly IProductsService productsService;

		public Products(IProductsService productsService)
		{
			this.productsService = productsService;
		}

		[FunctionName("GetProducts")]
		public async Task<IActionResult> GetProducts(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
			ILogger log)
		{
			var products = await this.productsService.ReadProducts();

			return new OkObjectResult(products.ToArray());
		}
	}
}
