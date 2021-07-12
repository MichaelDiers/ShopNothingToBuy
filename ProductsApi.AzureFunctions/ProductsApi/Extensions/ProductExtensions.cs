namespace ProductsApi.Extensions
{
	using System;
	using ProductsApi.Models;

	/// <summary>
	///   Extensions for <see cref="Product" /> objects.
	/// </summary>
	public static class ProductExtensions
	{
		/// <summary>
		///   Create an <see cref="ProductDTO" /> from the given <see cref="Product" />.
		/// </summary>
		/// <param name="product">The dto is created from this product.</param>
		/// <returns>An instance of <see cref="ProductDTO" />.</returns>
		public static ProductDTO ToDTO(this Product product)
		{
			return new ProductDTO(product);
		}

		/// <summary>
		///   Checks if a product is valid and therefore json serializable.
		///   It checks if the json attributes of <see cref="Product" /> are satisfied.
		/// </summary>
		/// <param name="product">The product to be checked.</param>
		/// <param name="hasId">Specifies if the id should be checked (true) or ignored (false).</param>
		/// <returns></returns>
		public static bool IsValid(this Product product, bool hasId = true)
		{
			return product != null
			       && !string.IsNullOrWhiteSpace(product.Description)
			       && !string.IsNullOrWhiteSpace(product.Name)
			       && (!hasId || product.Id != Guid.Empty);
		}
	}
}