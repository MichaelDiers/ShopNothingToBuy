namespace ProductsApi.Extensions
{
	using ProductsApi.Models;

	/// <summary>
	/// Extensions for <see cref="Product"/> objects.
	/// </summary>
	public static class ProductExtensions
	{
		/// <summary>
		/// Create an <see cref="ProductDTO"/> from the given <see cref="Product"/>.
		/// </summary>
		/// <param name="product">The dto is created from this product.</param>
		/// <returns>An instance of <see cref="ProductDTO"/>.</returns>
		public static ProductDTO ToDTO(this Product product)
		{
			return new ProductDTO(product);
		}
	}
}
