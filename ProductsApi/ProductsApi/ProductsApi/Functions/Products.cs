namespace ProductsApi
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Azure.WebJobs;
	using Microsoft.Azure.WebJobs.Extensions.Http;
	using Microsoft.Extensions.Logging;

	using Newtonsoft.Json;

	using ProductsApi.Contracts;
	using ProductsApi.Models;

	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	public class Products
	{
		private readonly IProductsService productsService;

		public Products(IProductsService productsService)
		{
			this.productsService = productsService;
		}

		[FunctionName("PostProducts")]
		public async Task<IActionResult> PostProducts(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest req,
			ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
			var createdProduct = await productsService.Create(productDTO);
			if (createdProduct != null)
			{
				return new CreatedResult(new Uri($"http://{req.Host.Value}/api/products/{createdProduct.Id}"), createdProduct);
			}

			return new BadRequestResult();
		}

		[FunctionName("DeleteProducts")]
		public async Task<IActionResult> DeleteProducts(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{id}")] HttpRequest req,
			ILogger log, string id)
		{
			var guid = new Guid(id);
			var isDeleted = await productsService.Delete(guid);
			if (isDeleted)
			{
				return new NoContentResult();
			}

			return new BadRequestResult();
		}

		[FunctionName("GetProducts")]
		public async Task<IActionResult> GetProducts(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
			ILogger log)
		{
			var products = await productsService.ListProducts();

			return new OkObjectResult(products.ToArray());
		}

		[FunctionName("GetProductsById")]
		public async Task<IActionResult> GetProductsById(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequest req,
			ILogger log, string id)
		{
			var guid = new Guid(id);
			var product = await productsService.ReadById(guid);
			return new OkObjectResult(product);
		}

		[FunctionName("PutProducts")]
		public async Task<IActionResult> PutProducts(
			[HttpTrigger(AuthorizationLevel.Function, "put", Route = "products")] HttpRequest req,
			ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
			var isUpdated = await productsService.Update(productDTO);
			if (isUpdated)
			{
				return new OkResult();
			}
			else
			{
				return new BadRequestResult();
			}
		}
	}
}
