namespace ProductsApi.Tests.Extensions
{
	using System;
	using ProductsApi.Extensions;
	using ProductsApi.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="ProductsApi.Extensions.ProductExtensions" />.
	/// </summary>
	public class ProductExtensionsTests
	{
		/// <summary>
		///   Used and expected description.
		/// </summary>
		private const string Description = "My Description";

		/// <summary>
		///   Used and expected name.
		/// </summary>
		private const string Name = "My Name";

		/// <summary>
		///   Used and expected id.
		/// </summary>
		private readonly Guid id = Guid.NewGuid();

		/// <summary>
		///   Tests the conversion from <see cref="Product" /> to <see cref="ProductDto" />.
		/// </summary>
		[Fact]
		public void ToDto()
		{
			var product = new Product {Description = Description, Name = Name, Id = this.id};
			var productDto = product.ToDto();

			productDto.ProductEqual(product, Description, Name, this.id);
		}
	}
}