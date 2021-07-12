namespace ProductsApi.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Logging;
	using ProductsApi.Models;

	/// <summary>
	///   Provides functionality of a service for handling products.
	/// </summary>
	public interface IProductsService
	{
		/// <summary>
		///   Delete all items from the database container.
		/// </summary>
		/// <param name="log">
		///   ///
		///   <param name="log">An <see cref="ILogger" /> instance.</param>
		/// </param>
		/// <returns>True if all entries are deleted, otherwise false.</returns>
		Task<bool> Clear(ILogger log);

		/// <summary>
		///   Creates a new product.
		/// </summary>
		/// <param name="productDTO">The product to be created.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>The product as <see cref="ProductDTO" /> or null if the operation failed.</returns>
		Task<ProductDTO> Create(ProductDTO productDTO, ILogger log);

		/// <summary>
		///   Delete a product.
		/// </summary>
		/// <param name="id">The id of the product to be deleted.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>True if the product is deleted, false if no product with the given <paramref name="id" /> exists.</returns>
		Task<bool> Delete(Guid id, ILogger log);

		/// <summary>
		///   Retrieve a list of all known products.
		/// </summary>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>A list of all products.</returns>
		Task<IEnumerable<ProductDTO>> ListProducts(ILogger log);

		/// <summary>
		///   Reads a product by the given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the product.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>The product or null if no product with <paramref name="id" /> exists.</returns>
		Task<ProductDTO> ReadById(Guid id, ILogger log);

		/// <summary>
		///   Updates an existing product.
		/// </summary>
		/// <param name="productDTO">The new values for the product.</param>
		/// <param name="log">An <see cref="ILogger" /> instance.</param>
		/// <returns>True if the product is updated and false if no product with <see cref="ProductDTO.Id" /> exists.</returns>
		Task<bool> Update(ProductDTO productDTO, ILogger log);
	}
}