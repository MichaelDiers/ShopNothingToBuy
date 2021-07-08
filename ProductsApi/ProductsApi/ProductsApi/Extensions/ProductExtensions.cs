namespace ProductsApi.Extensions
{
	using ProductsApi.Contracts;
	using ProductsApi.Models;

	public static class ProductExtensions
	{
		public static IProductDTO ToDTO(this IProduct product)
		{
			return new ProductDTO(product);
		}
	}
}
