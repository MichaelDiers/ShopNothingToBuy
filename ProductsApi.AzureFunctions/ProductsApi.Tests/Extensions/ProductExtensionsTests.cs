namespace ProductsApi.Tests.Extensions
{
	using ProductsApi.Tests;
	using ProductsApi.Extensions;
	using ProductsApi.Models;

	using System;

	using Xunit;

	/// <summary>
	/// Tests for <see cref="ProductsApi.Extensions.ProductExtensions"/>.
	/// </summary>
	public class ProductExtensionsTests
	{
		/// <summary>
		/// Used and expected description.
		/// </summary>
		private const string Description = "My Description";

		/// <summary>
		/// Used and expected name.
		/// </summary>
		private const string Name = "My Name";

		/// <summary>
		/// Used and expected id.
		/// </summary>
		private readonly Guid Id = Guid.NewGuid();

		/// <summary>
		/// Tests the conversion from <see cref="Product"/> to <see cref="ProductDTO"/>.
		/// </summary>
		[Fact]
		public void ToDTO()
		{
			var product = new Product { Description = Description, Name = Name, Id = Id };
			var productDTO = product.ToDTO();

			productDTO.ProductEqual(product, Description, Name, Id);
		}
	}
}
