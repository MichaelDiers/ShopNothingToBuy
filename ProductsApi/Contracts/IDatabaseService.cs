namespace ProductsApi.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides database operations for objects of type <see cref="IProduct"/>.
	/// </summary>
	public interface IDatabaseService
	{
		/// <summary>
		/// Creates a new product in the database.
		/// </summary>
		/// <param name="product">The product to be created.</param>
		/// <returns>True if the product is created, false if an entry with <see cref="IProduct.Id"/> already exists.</returns>
		Task<bool> Create(IProduct product);

		/// <summary>
		/// Delete a product from the database.
		/// </summary>
		/// <param name="id">The id of the product to be deleted.</param>
		/// <returns>True if the product is deleted, false if no product with the given <paramref name="id"/> exists.</returns>
		Task<bool> Delete(Guid id);

		/// <summary>
		/// Retrieve a list of all products in the database.
		/// </summary>
		/// <returns>A list of all products.</returns>
		Task<IEnumerable<IProduct>> List();

		/// <summary>
		/// Reads a product by the given <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The id of the product.</param>
		/// <returns>The product or null if no product with <paramref name="id"/> exists.</returns>
		Task<IProduct> ReadById(Guid id);

		/// <summary>
		/// Updates an existing product.
		/// </summary>
		/// <param name="product">The new values for the product.</param>
		/// <returns>True if the product is updated and false if no product with <see cref="IProduct.Id"/> exists.</returns>
		Task<bool> Update(IProduct product);
	}
}
