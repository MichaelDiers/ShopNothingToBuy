namespace ProductsApi.Functions
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Azure.WebJobs;
	using Microsoft.Azure.WebJobs.Extensions.Http;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
	using ProductsApi.Contracts;
	using ProductsApi.Models;

	public class Products
	{
		private const string PostProductsLocationFormatName = "postProductsLocationFormat";
		private readonly IApiKeyService apiKeyService;
		private readonly string postProductsLocationFormat;
		private readonly IProductsService productsService;

		public Products(IProductsService productsService, IApiKeyService apiKeyService, IConfiguration configuration)
		{
			this.productsService = productsService;
			this.apiKeyService = apiKeyService;
			this.postProductsLocationFormat = configuration.GetValue<string>(PostProductsLocationFormatName);
		}

		[FunctionName("DeleteProductsClearAll")]
		public async Task<IActionResult> DeleteProductsClearAll(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/clear/all")]
			HttpRequest req,
			ILogger log)
		{
			// remove try-catch as soon as FunctionExceptionFilterAttribute or an alternative is available
			try
			{
				// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
				if (!this.apiKeyService.IsValid(req))
				{
					return new UnauthorizedResult();
				}

				if (await this.productsService.Clear(log))
				{
					return new NoContentResult();
				}

				return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
			}
			catch (Exception ex)
			{
				log.LogError(ex.ToString());
				return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
			}
		}

		[FunctionName("PostProducts")]
		public async Task<IActionResult> PostProducts(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")]
			HttpRequest req,
			ILogger log)
		{
			// remove try-catch as soon as FunctionExceptionFilterAttribute or an alternative is available
			try
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

				var createdProduct = await this.productsService.Create(productDTO, log);
				if (createdProduct != null)
				{
					return new CreatedResult(
						new Uri(string.Format(this.postProductsLocationFormat, req.Host.Value, createdProduct.Id)),
						createdProduct);
				}

				return new BadRequestResult();
			}
			catch (Exception ex)
			{
				log.LogError(ex.ToString());
				return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
			}
		}

		[FunctionName("DeleteProducts")]
		public async Task<IActionResult> DeleteProducts(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{id}")]
			HttpRequest req,
			ILogger log,
			string id)
		{
			// remove try-catch as soon as FunctionExceptionFilterAttribute or an alternative is available
			try
			{
				// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
				if (!this.apiKeyService.IsValid(req))
				{
					return new UnauthorizedResult();
				}

				var isValid = Guid.TryParse(id, out var guid);
				if (!isValid || guid == Guid.Empty)
				{
					return new BadRequestResult();
				}

				var isDeleted = await this.productsService.Delete(guid, log);
				if (isDeleted)
				{
					return new NoContentResult();
				}

				return new NotFoundResult();
			}
			catch (Exception ex)
			{
				log.LogError(ex.ToString());
				return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
			}
		}

		[FunctionName("GetProducts")]
		public async Task<IActionResult> GetProducts(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")]
			HttpRequest req,
			ILogger log)
		{
			// remove try-catch as soon as FunctionExceptionFilterAttribute or an alternative is available
			try
			{
				// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
				if (!this.apiKeyService.IsValid(req))
				{
					return new UnauthorizedResult();
				}

				var products = await this.productsService.ListProducts(log);

				return new OkObjectResult(products.ToArray());
			}
			catch (Exception ex)
			{
				log.LogError(ex.ToString());
				return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
			}
		}

		[FunctionName("GetProductsById")]
		public async Task<IActionResult> GetProductsById(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")]
			HttpRequest req,
			ILogger log,
			string id)
		{
			// remove try-catch as soon as FunctionExceptionFilterAttribute or an alternative is available
			try
			{
				// use FunctionInvocationFilterAttribute as soon as microsoft does not flag it with deprecated
				if (!this.apiKeyService.IsValid(req))
				{
					return new UnauthorizedResult();
				}

				var isValid = Guid.TryParse(id, out var guid);
				if (isValid && guid != Guid.Empty)
				{
					var product = await this.productsService.ReadById(guid, log);
					if (product != null)
					{
						return new OkObjectResult(product);
					}
				}

				return new NotFoundResult();
			}
			catch (Exception ex)
			{
				log.LogError(ex.ToString());
				return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
			}
		}

		[FunctionName("PutProducts")]
		public async Task<IActionResult> PutProducts(
			[HttpTrigger(AuthorizationLevel.Function, "put", Route = "products")]
			HttpRequest req,
			ILogger log)
		{
			// remove try-catch as soon as FunctionExceptionFilterAttribute or an alternative is available
			try
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

				var isUpdated = await this.productsService.Update(productDTO, log);
				if (isUpdated)
				{
					return new OkResult();
				}

				return new NotFoundResult();
			}
			catch (Exception ex)
			{
				log.LogError(ex.ToString());
				return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
			}
		}

		private static async Task<T> DeserializeBody<T>(Stream body, ILogger log) where T : class
		{
			try
			{
				var requestBody = await new StreamReader(body).ReadToEndAsync();
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