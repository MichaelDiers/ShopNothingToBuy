namespace ProductsApi.Tests.Extensions
{
	using System;
	using Newtonsoft.Json;
	using ProductsApi.Extensions;
	using ProductsApi.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="ProductDtoExtensions" />.
	/// </summary>
	public class ProductDtoExtensionsTests
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
		///   Tests the conversion from <see cref="ProductDto" /> to <see cref="Product" />.
		/// </summary>
		[Fact]
		public void FromDto()
		{
			var productDto = new ProductDto {Description = Description, Name = Name, Id = this.id};
			var product = productDto.FromDto();

			productDto.ProductEqual(product, Description, Name, this.id);
		}

		/// <summary>
		///   Tests the conversion from <see cref="ProductDto" /> to <see cref="Product" /> using additional id.
		/// </summary>
		[Fact]
		public void FromDtoWithGuid()
		{
			var productDto = new ProductDto {Description = Description, Name = Name};
			var product = productDto.FromDto(this.id);

			Assert.Equal(productDto.Description, product.Description);
			Assert.Equal(productDto.Name, product.Name);
			Assert.NotEqual(productDto.Id, product.Id);

			var productJson = JsonConvert.SerializeObject(product);
			var productDtoJson = JsonConvert.SerializeObject(productDto);
			Assert.NotEqual(productJson, productDtoJson);

			Assert.Equal(Description, product.Description);
			Assert.Equal(Name, product.Name);
			Assert.Equal(this.id, product.Id);
		}
	}
}