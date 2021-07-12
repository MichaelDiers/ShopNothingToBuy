namespace ProductsApi.Tests.Extensions
{
	using System;
	using Newtonsoft.Json;
	using ProductsApi.Extensions;
	using ProductsApi.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="ProductsApi.Extensions.ProductDTOExtensions" />.
	/// </summary>
	public class ProductDTOExtensionsTests
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
		private readonly Guid Id = Guid.NewGuid();

		/// <summary>
		///   Tests the conversion from <see cref="ProductDTO" /> to <see cref="Product" />.
		/// </summary>
		[Fact]
		public void FromDTO()
		{
			var productDTO = new ProductDTO {Description = Description, Name = Name, Id = this.Id};
			var product = productDTO.FromDTO();

			productDTO.ProductEqual(product, Description, Name, this.Id);
		}

		/// <summary>
		///   Tests the conversion from <see cref="ProductDTO" /> to <see cref="Product" /> using additional id.
		/// </summary>
		[Fact]
		public void FromDTOWithGuid()
		{
			var productDTO = new ProductDTO {Description = Description, Name = Name};
			var product = productDTO.FromDTO(this.Id);

			Assert.Equal(productDTO.Description, product.Description);
			Assert.Equal(productDTO.Name, product.Name);
			Assert.NotEqual(productDTO.Id, product.Id);

			var productJson = JsonConvert.SerializeObject(product);
			var productDTOJson = JsonConvert.SerializeObject(productDTO);
			Assert.NotEqual(productJson, productDTOJson);

			Assert.Equal(Description, product.Description);
			Assert.Equal(Name, product.Name);
			Assert.Equal(this.Id, product.Id);
		}
	}
}