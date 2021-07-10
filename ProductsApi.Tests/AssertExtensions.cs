namespace ProductsApi.Tests
{
	using System;

	using ProductsApi.Models;

	using Xunit;
	using Newtonsoft.Json;

	/// <summary>
	/// Extensions used for asserting test objects.
	/// </summary>
	public static class AssertExtensions
	{
		/// <summary>
		/// Verify that the given <paramref name="product"/> and <paramref name="productDTO"/> are equal.
		/// </summary>
		/// <param name="productDTO">A data transfer object.</param>
		/// <param name="product">An application object.</param>
		public static void ProductEqual(this ProductDTO productDTO, Product product)
		{
			Assert.Equal(productDTO.Description, product.Description);
			Assert.Equal(productDTO.Name, product.Name);
			Assert.Equal(productDTO.Id, product.Id);

			var productJson = JsonConvert.SerializeObject(product);
			var productDTOJson = JsonConvert.SerializeObject(productDTO);
			Assert.Equal(productJson, productDTOJson);
		}

		/// <summary>
		/// Verify that the given <paramref name="product"/> and <paramref name="productDTO"/> are equal.
		/// Additional checks for <paramref name="description"/>, <paramref name="id"/> and <paramref name="name"/>
		/// for <paramref name="product"/> and <paramref name="productDTO"/>.
		/// </summary>
		/// <param name="productDTO">A data transfer object.</param>
		/// <param name="product">An application object.</param>
		/// <param name="description">The expected description in <paramref name="product"/> and <paramref name="productDTO"/>.</param>
		/// /// <param name="name">The expected name in <paramref name="product"/> and <paramref name="productDTO"/>.</param>
		/// /// <param name="id">The expected id in <paramref name="product"/> and <paramref name="productDTO"/>.</param>
		public static void ProductEqual(this ProductDTO productDTO, Product product, string description, string name, Guid id)
		{
			productDTO.ProductEqual(product);

			Assert.Equal(description, product.Description);
			Assert.Equal(name, product.Name);
			Assert.Equal(id, product.Id);

			Assert.Equal(description, productDTO.Description);
			Assert.Equal(name, productDTO.Name);
			Assert.Equal(id, productDTO.Id);
		}
	}
}
