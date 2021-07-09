namespace ProductsApi.Extensions
{
	using ProductsApi.Contracts;
	using ProductsApi.Models;

	using System;

	/// <summary>
	/// Extensions for <see cref="ProductDTO"/> objects.
	/// </summary>
	public static class ProductDTOExtensions
	{
		/// <summary>
		/// Create a new <see cref="Product"/> from the given <see cref="ProductDTO"/>.
		/// </summary>
		/// <param name="productDTO">The dto from that the product is created.</param>
		/// <returns>An instance of <see cref="Product"/>.</returns>
		public static Product FromDTO(this ProductDTO productDTO)
		{
			return new Product(productDTO);
		}

		/// <summary>
		/// Create a new <see cref="Product"/> from the given <see cref="ProductDTO"/>.
		/// </summary>
		/// <param name="productDTO">The dto from that the product is created.</param>
		/// <param name="id">The <see cref="Product.Id"/> of the new <see cref="Product"/> object will be set to this id.</param>
		/// <returns>An instance of <see cref="Product"/>.</returns>
		public static Product FromDTO(this ProductDTO productDTO, Guid id)
		{
			return new Product(productDTO) { Id = id };
		}
	}
}
