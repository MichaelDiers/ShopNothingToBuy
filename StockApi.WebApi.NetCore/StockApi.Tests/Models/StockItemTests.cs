namespace StockApi.Tests.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using StockApi.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="StockItem" />.
	/// </summary>
	public class StockItemTests
	{
		/// <summary>
		///   Should succeed for valid parameters.
		/// </summary>
		[Fact]
		public void ConstructorShouldSucceedForValidDto()
		{
			var id = Guid.NewGuid();
			const int inStock = 100;
			var model = new StockItemDto {Id = id, InStock = inStock};
			var stockItem = new StockItem(model);
			Assert.Equal(inStock, stockItem.InStock);
			Assert.Equal(id, stockItem.Id);
		}

		/// <summary>
		///   Should fail if parameter is null.
		/// </summary>
		[Fact]
		public void ConstructorShouldThrowArgumentNullExceptionIfDtoIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new StockItem(null));
		}

		/// <summary>
		///   Should fail if <see cref="StockItemDto.Id" /> is null.
		/// </summary>
		[Fact]
		public void ConstructorShouldThrowArgumentNullExceptionIfIdIsNull()
		{
			var model = new StockItemDto {InStock = 10};
			Assert.Throws<ArgumentNullException>(() => new StockItem(model));
		}

		/// <summary>
		///   Should fail if <see cref="StockItemDto.InStock" /> is null.
		/// </summary>
		[Fact]
		public void ConstructorShouldThrowArgumentNullExceptionIfInStockIsNull()
		{
			var model = new StockItemDto {Id = Guid.NewGuid()};
			Assert.Throws<ArgumentNullException>(() => new StockItem(model));
		}

		/// <summary>
		///   Dummy test for calling the default constructor.
		/// </summary>
		[Fact]
		public void DefaultConstructorTest()
		{
			var model = new StockItem();
			Assert.NotNull(model);
		}

		/// <summary>
		///   Validation should fail if <see cref="StockItem.Id" /> equals <see cref="Guid.Empty" />.
		/// </summary>
		[Fact]
		public void NonEmptyGuidValidationForId()
		{
			var model = new StockItem {Id = Guid.Empty, InStock = 10};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation should fail if <see cref="StockItem.InStock" /> is greater than 10001.
		/// </summary>
		[Fact]
		public void RangeTooHighValidationForInStock()
		{
			var model = new StockItem {Id = Guid.NewGuid(), InStock = 10001};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation fails for invalid id.
		/// </summary>
		[Fact]
		public void RequiredValidationForId()
		{
			var model = new StockItem {InStock = 10};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation should succeed for valid <see cref="StockItem" /> values.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <param name="inStock">The stock of the <see cref="StockItem" />.</param>
		[Theory]
		[InlineData("7c3eea8b-01a4-4195-88b7-2323310e1c7d", 0)]
		[InlineData("7c3eea8b-01a4-4195-88b7-2323310e1c7d", 10000)]
		public void ValidationSuccess(string id, int inStock)
		{
			var model = new StockItem {Id = new Guid(id), InStock = inStock};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.True(isValid);
		}
	}
}