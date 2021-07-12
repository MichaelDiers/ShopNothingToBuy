namespace ProductsApi.Extensions
{
	using System;
	using ProductsApi.Models;

	/// <summary>
	///   Extensions for <see cref="ProductDto" /> objects.
	/// </summary>
	public static class ProductDtoExtensions
	{
		/// <summary>
		///   Create a new <see cref="Product" /> from the given <see cref="ProductDto" />.
		/// </summary>
		/// <param name="productDto">The dto from that the product is created.</param>
		/// <returns>An instance of <see cref="Product" />.</returns>
		public static Product FromDto(this ProductDto productDto)
		{
			return new Product(productDto);
		}

		/// <summary>
		///   Create a new <see cref="Product" /> from the given <see cref="ProductDto" />.
		/// </summary>
		/// <param name="productDto">The dto from that the product is created.</param>
		/// <param name="id">The <see cref="Product.Id" /> of the new <see cref="Product" /> object will be set to this id.</param>
		/// <returns>An instance of <see cref="Product" />.</returns>
		public static Product FromDto(this ProductDto productDto, Guid id)
		{
			return new Product(productDto) {Id = id};
		}
	}
}