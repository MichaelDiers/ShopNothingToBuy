namespace ProductsApi.Extensions
{
	using ProductsApi.Contracts;
	using ProductsApi.Models;

	using System;

	/// <summary>
	/// Extensions for <see cref="IProduct"/> objects.
	/// </summary>
	public static class ProductExtensions
	{
		/// <summary>
		/// Create an <see cref="IProductDTO"/> from the given <see cref="IProduct"/>.
		/// </summary>
		/// <param name="product">The dto is created from this product.</param>
		/// <returns>An instance of <see cref="IProductDTO"/>.</returns>
		public static IProductDTO ToDTO(this IProduct product)
		{
			return new ProductDTO(product);
		}		
	}
}
