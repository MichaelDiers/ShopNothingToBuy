namespace StockApi.Tests.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using StockApi.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="StockItemDto" />.
	/// </summary>
	public class StockItemDtoTests
	{
		/// <summary>
		///   Dummy test for calling the default constructor.
		/// </summary>
		[Fact]
		public void DefaultConstructorTest()
		{
			var model = new StockItemDto();
			Assert.NotNull(model);
		}

		/// <summary>
		///   Should fail if parameter is null.
		/// </summary>
		[Fact]
		public void ConstructorShouldThrowArgumentNullExceptionIfParameterIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new StockItemDto(null));
		}

		/// <summary>
		///   Should succeed for valid parameters.
		/// </summary>
		[Fact]
		public void ConstructorShouldSucceedForValidParameter()
		{
			var id = Guid.NewGuid();
			const int inStock = 100;
			var model = new StockItem {Id = id, InStock = inStock};
			var stockItem = new StockItemDto(model);
			Assert.Equal(inStock, stockItem.InStock);
			Assert.Equal(id, stockItem.Id);
		}

		/// <summary>
		///   Validation fails for invalid id.
		/// </summary>
		[Fact]
		public void RequiredValidationForId()
		{
			var model = new StockItemDto {InStock = 10};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation should fail if <see cref="StockItemDto.Id" /> equals <see cref="Guid.Empty" />.
		/// </summary>
		[Fact]
		public void NonEmptyGuidValidationForId()
		{
			var model = new StockItemDto {Id = Guid.Empty, InStock = 10};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation should fail if <see cref="StockItemDto.InStock" /> is not set.
		/// </summary>
		[Fact]
		public void RequiredValidationForInStock()
		{
			var model = new StockItemDto {Id = Guid.NewGuid()};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation should fail if <see cref="StockItemDto.InStock" /> is lower than 0.
		/// </summary>
		[Fact]
		public void RangeTooLowValidationForInStock()
		{
			var model = new StockItemDto {Id = Guid.NewGuid(), InStock = -1};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation should fail if <see cref="StockItemDto.InStock" /> is greater than 10001.
		/// </summary>
		[Fact]
		public void RangeTooHighValidationForInStock()
		{
			var model = new StockItemDto {Id = Guid.NewGuid(), InStock = 10001};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.False(isValid);
		}

		/// <summary>
		///   Validation should succeed for valid <see cref="StockItemDto" /> values.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItemDto" />.</param>
		/// <param name="inStock">The stock of the <see cref="StockItemDto" />.</param>
		[Theory]
		[InlineData("7c3eea8b-01a4-4195-88b7-2323310e1c7d", 0)]
		[InlineData("7c3eea8b-01a4-4195-88b7-2323310e1c7d", 10000)]
		public void ValidationSuccess(string id, int inStock)
		{
			var model = new StockItemDto {Id = new Guid(id), InStock = inStock};
			var context = new ValidationContext(model);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(model, context, results, true);
			Assert.True(isValid);
		}
	}
}