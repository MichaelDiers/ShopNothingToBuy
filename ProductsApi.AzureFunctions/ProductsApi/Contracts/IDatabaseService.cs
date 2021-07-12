namespace ProductsApi.Contracts
{
	using Microsoft.Extensions.Logging;

	using ProductsApi.Models;

	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides database operations for objects of type <see cref="Product"/>.
	/// </summary>
	public interface IDatabaseService
	{
		/// <summary>
		/// Delete all items from the database container.
		/// </summary>
		/// <param name="log">/// <param name="log">An <see cref="ILogger"/> instance.</param></param>
		/// <returns>True if all entries are deleted, otherwise false.</returns>
		Task<bool> Clear(ILogger log);

		/// <summary>
		/// Creates a new product in the database.
		/// </summary>
		/// <param name="product">The product to be created.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>True if the product is created, false if an entry with <see cref="Product.Id"/> already exists.</returns>
		Task<bool> Create(Product product, ILogger log);

		/// <summary>
		/// Delete a product from the database.
		/// </summary>
		/// <param name="id">The id of the product to be deleted.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>True if the product is deleted, false if no product with the given <paramref name="id"/> exists.</returns>
		Task<bool> Delete(Guid id, ILogger log);

		/// <summary>
		/// Retrieve a list of all products in the database.
		/// </summary>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <param name="validProductsOnly">True specifies that only <see cref="Product"/> objects are 
		///		loaded that satisfy the formal json definition of the <see cref="Product"/>, otherwise
		///		all products are loaded - even if not json serializable.
		/// </param>
		/// <returns>A list of all products.</returns>
		Task<IEnumerable<Product>> List(ILogger log, bool validProductsOnly = true);

		/// <summary>
		/// Reads a product by the given <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The id of the product.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>The product or null if no product with <paramref name="id"/> exists.</returns>
		Task<Product> ReadById(Guid id, ILogger log);

		/// <summary>
		/// Updates an existing product.
		/// </summary>
		/// <param name="product">The new values for the product.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>True if the product is updated and false if no product with <see cref="Product.Id"/> exists.</returns>
		Task<bool> Update(Product product, ILogger log);
	}
}
