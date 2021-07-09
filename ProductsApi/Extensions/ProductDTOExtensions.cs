namespace ProductsApi.Extensions
{
	using ProductsApi.Contracts;
	using ProductsApi.Models;

	using System;

	/// <summary>
	/// Extensions for <see cref="IProductDTO"/> objects.
	/// </summary>
	public static class ProductDTOExtensions
	{
		/// <summary>
		/// Create a new <see cref="IProduct"/> from the given <see cref="IProductDTO"/>.
		/// </summary>
		/// <param name="productDTO">The dto from that the product is created.</param>
		/// <returns>An instance of <see cref="IProduct"/>.</returns>
		public static IProduct FromDTO(this IProductDTO productDTO)
		{
			return new Product(productDTO);
		}

		/// <summary>
		/// Create a new <see cref="IProduct"/> from the given <see cref="IProductDTO"/>.
		/// </summary>
		/// <param name="productDTO">The dto from that the product is created.</param>
		/// <param name="id">The <see cref="IProduct.Id"/> of the new <see cref="IProduct"/> object will be set to this id.</param>
		/// <returns>An instance of <see cref="IProduct"/>.</returns>
		public static IProduct FromDTO(this IProductDTO productDTO, Guid id)
		{
			return new Product(productDTO) { Id = id };
		}
	}
}
