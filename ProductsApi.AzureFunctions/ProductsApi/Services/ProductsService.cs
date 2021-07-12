namespace ProductsApi.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Logging;
	using ProductsApi.Contracts;
	using ProductsApi.Extensions;
	using ProductsApi.Models;

	/// <summary>
	///   Provides functionality of a service for handling products.
	/// </summary>
	public class ProductsService : IProductsService
	{
		/// <summary>
		///   The service used for database operations.
		/// </summary>
		private readonly IDatabaseService databaseService;

		/// <summary>
		///   Creates a new instance of <see cref="ProductsService" />.
		/// </summary>
		/// <param name="databaseService">The service used for database operations.</param>
		public ProductsService(IDatabaseService databaseService)
		{
			this.databaseService = databaseService;
		}

		/// <summary>
		///   Delete all items from the database container.
		/// </summary>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>True if all entries are deleted, otherwise false.</returns>
		public async Task<bool> Clear(ILogger log)
		{
			return await this.databaseService.Clear(log);
		}

		/// <summary>
		///   Creates a new product.
		/// </summary>
		/// <param name="productDTO">The product to be created.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>The product as <see cref="ProductDTO" /> or null if the operation failed.</returns>
		public async Task<ProductDTO> Create(ProductDTO productDTO, ILogger log)
		{
			var product = productDTO.FromDTO(Guid.NewGuid());
			if (await this.databaseService.Create(product, log))
			{
				return product.ToDTO();
			}

			return null;
		}

		/// <summary>
		///   Delete a product.
		/// </summary>
		/// <param name="id">The id of the product to be deleted.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>True if the product is deleted, false if no product with the given <paramref name="id" /> exists.</returns>
		public async Task<bool> Delete(Guid id, ILogger log)
		{
			return await this.databaseService.Delete(id, log);
		}

		/// <summary>
		///   Retrieve a list of all known products.
		/// </summary>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>A list of all products.</returns>
		public async Task<IEnumerable<ProductDTO>> ListProducts(ILogger log)
		{
			var products = await this.databaseService.List(log);
			return products.Select(product => product.ToDTO());
		}

		/// <summary>
		///   Reads a product by the given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the product.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>The product or null if no product with <paramref name="id" /> exists.</returns>
		public async Task<ProductDTO> ReadById(Guid id, ILogger log)
		{
			var product = await this.databaseService.ReadById(id, log);
			return product?.ToDTO();
		}

		/// <summary>
		///   Updates an existing product.
		/// </summary>
		/// <param name="product">The new values for the product.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>True if the product is updated and false if no product with <see cref="ProductDTO.Id" /> exists.</returns>
		public async Task<bool> Update(ProductDTO productDTO, ILogger log)
		{
			var product = productDTO.FromDTO();
			return await this.databaseService.Update(product, log);
		}
	}
}