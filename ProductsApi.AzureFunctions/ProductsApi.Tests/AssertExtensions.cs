namespace ProductsApi.Tests
{
	using System;
	using Newtonsoft.Json;
	using ProductsApi.Models;
	using Xunit;

	/// <summary>
	///   Extensions used for asserting test objects.
	/// </summary>
	public static class AssertExtensions
	{
		/// <summary>
		///   Verify that the given <paramref name="product" /> and <paramref name="productDto" /> are equal.
		/// </summary>
		/// <param name="productDto">A data transfer object.</param>
		/// <param name="product">An application object.</param>
		public static void ProductEqual(this ProductDto productDto, Product product)
		{
			Assert.Equal(productDto.Description, product.Description);
			Assert.Equal(productDto.Name, product.Name);
			Assert.Equal(productDto.Id, product.Id);

			var productJson = JsonConvert.SerializeObject(product);
			var productDtoJson = JsonConvert.SerializeObject(productDto);
			Assert.Equal(productJson, productDtoJson);
		}

		/// <summary>
		///   Verify that the given <paramref name="product" /> and <paramref name="productDto" /> are equal.
		///   Additional checks for <paramref name="description" />, <paramref name="id" /> and <paramref name="name" />
		///   for <paramref name="product" /> and <paramref name="productDto" />.
		/// </summary>
		/// <param name="productDto">A data transfer object.</param>
		/// <param name="product">An application object.</param>
		/// <param name="description">The expected description in <paramref name="product" /> and <paramref name="productDto" />.</param>
		/// ///
		/// <param name="name">The expected name in <paramref name="product" /> and <paramref name="productDto" />.</param>
		/// ///
		/// <param name="id">The expected id in <paramref name="product" /> and <paramref name="productDto" />.</param>
		public static void ProductEqual(this ProductDto productDto, Product product, string description, string name,
			Guid id)
		{
			productDto.ProductEqual(product);

			Assert.Equal(description, product.Description);
			Assert.Equal(name, product.Name);
			Assert.Equal(id, product.Id);

			Assert.Equal(description, productDto.Description);
			Assert.Equal(name, productDto.Name);
			Assert.Equal(id, productDto.Id);
		}
	}
}