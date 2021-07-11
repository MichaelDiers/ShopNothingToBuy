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
		private readonly IApiKeyService apiKeyService;

		public Products(IProductsService productsService, IApiKeyService apiKeyService)
		{
			this.productsService = productsService;
			this.apiKeyService = apiKeyService;
		}

		[FunctionName("PostProducts")]
		public async Task<IActionResult> PostProducts(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest req,
			ILogger log)			
		{
			// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
			if (!this.apiKeyService.IsValid(req))
			{
				return new UnauthorizedResult();
			}

			var productDTO = await DeserializeBody<ProductDTO>(req.Body, log);
			if (productDTO is null || productDTO.Id != Guid.Empty)
			{
				return new BadRequestResult();
			}
			
			var createdProduct = await productsService.Create(productDTO, log);
			if (createdProduct != null)
			{
				return new CreatedResult(new Uri($"http://{req.Host.Value}/api/products/{createdProduct.Id}"), createdProduct);
			}

			return new BadRequestResult();
		}

		[FunctionName("DeleteProducts")]
		public async Task<IActionResult> DeleteProducts(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{id}")] HttpRequest req,
			ILogger log,
			string id)
		{
			// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
			if (!this.apiKeyService.IsValid(req))
			{
				return new UnauthorizedResult();
			}

			var isValid = Guid.TryParse(id, out Guid guid);
			if (!isValid || guid == Guid.Empty)
			{
				return new BadRequestResult();
			}

			var isDeleted = await productsService.Delete(guid, log);
			if (isDeleted)
			{
				return new NoContentResult();
			}			
			else
			{
				return new NotFoundResult();
			}
		}

		[FunctionName("GetProducts")]
		public async Task<IActionResult> GetProducts(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
			ILogger log)
		{
			// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
			if (!this.apiKeyService.IsValid(req))
			{
				return new UnauthorizedResult();
			}

			var products = await productsService.ListProducts(log);

			return new OkObjectResult(products.ToArray());
		}

		[FunctionName("GetProductsById")]
		public async Task<IActionResult> GetProductsById(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequest req,
			ILogger log,
			string id)
		{
			// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
			if (!this.apiKeyService.IsValid(req))
			{
				return new UnauthorizedResult();
			}

			var isValid = Guid.TryParse(id, out Guid guid);
			if (isValid && guid != Guid.Empty)
			{
				var product = await productsService.ReadById(guid, log);
				if (product != null)
				{
					return new OkObjectResult(product);
				}
			}
			
			return new NotFoundResult();
		}

		[FunctionName("PutProducts")]
		public async Task<IActionResult> PutProducts(
			[HttpTrigger(AuthorizationLevel.Function, "put", Route = "products")] HttpRequest req,
			ILogger log)
		{
			// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
			if (!this.apiKeyService.IsValid(req))
			{
				return new UnauthorizedResult();
			}

			var productDTO = await DeserializeBody<ProductDTO>(req.Body, log);
			if (productDTO is null || productDTO.Id == Guid.Empty)
			{
				return new BadRequestResult();
			}
			
			var isUpdated = await productsService.Update(productDTO, log);
			if (isUpdated)
			{
				return new OkResult();
			}
			else
			{
				return new NotFoundResult();
			}
		}

		private static async Task<T> DeserializeBody<T>(Stream body, ILogger log) where T : class
		{
			try
			{
				string requestBody = await new StreamReader(body).ReadToEndAsync();
				if (string.IsNullOrWhiteSpace(requestBody))
				{
					return null;
				}

				var deserializeObject = JsonConvert.DeserializeObject<T>(requestBody);
				return deserializeObject;
			}
			catch (JsonSerializationException)
			{
				// do not handle missing request data
				// see json attributes on type T, for example see ProductDTO
			}
			catch (Exception ex)
			{
				log.LogError(ex.ToString());
			}

			return null;
		}
	}
}
