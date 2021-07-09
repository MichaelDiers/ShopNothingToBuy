namespace ProductsApi.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides functionality of a service for handling products.
	/// </summary>
	public interface IProductsService
	{
		/// <summary>
		/// Creates a new product.
		/// </summary>
		/// <param name="productDTO">The product to be created.</param>
		/// <returns>The product as <see cref="IProductDTO"/> or null if the operation failed.</returns>
		Task<IProductDTO> Create(IProductDTO productDTO);

		/// <summary>
		/// Delete a product.
		/// </summary>
		/// <param name="id">The id of the product to be deleted.</param>
		/// <returns>True if the product is deleted, false if no product with the given <paramref name="id"/> exists.</returns>
		Task<bool> Delete(Guid id);

		/// <summary>
		/// Retrieve a list of all known products.
		/// </summary>
		/// <returns>A list of all products.</returns>
		Task<IEnumerable<IProductDTO>> ListProducts();

		/// <summary>
		/// Reads a product by the given <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The id of the product.</param>
		/// <returns>The product or null if no product with <paramref name="id"/> exists.</returns>
		Task<IProductDTO> ReadById(Guid id);

		/// <summary>
		/// Updates an existing product.
		/// </summary>
		/// <param name="product">The new values for the product.</param>
		/// <returns>True if the product is updated and false if no product with <see cref="IProductDTO.Id"/> exists.</returns>
		Task<bool> Update(IProductDTO productDTO);
	}
}
