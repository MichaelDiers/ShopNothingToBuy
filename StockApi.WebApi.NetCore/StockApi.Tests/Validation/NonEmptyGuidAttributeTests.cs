namespace StockApi.Tests.Validation
{
	using System;
	using StockApi.Validation;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="NonEmptyGuidAttribute" />.
	/// </summary>
	public class NonEmptyGuidAttributeTests
	{
		/// <summary>
		///   <see cref="NonEmptyGuidAttribute.IsValid" /> should return true for non-empty <see cref="Guid" />.
		/// </summary>
		[Fact]
		public void IsValidReturnsTrueForNonEmptyGuid()
		{
			var guid = Guid.NewGuid();
			var attribute = new NonEmptyGuidAttribute();
			Assert.True(attribute.IsValid(guid));
		}

		/// <summary>
		///   <see cref="NonEmptyGuidAttribute.IsValid" /> should return false for empty <see cref="Guid" />.
		/// </summary>
		[Fact]
		public void IsValidReturnsFalseForEmptyGuid()
		{
			var guid = Guid.Empty;
			var attribute = new NonEmptyGuidAttribute();
			Assert.False(new NonEmptyGuidAttribute().IsValid(guid));
		}

		/// <summary>
		///   <see cref="NonEmptyGuidAttribute.IsValid" /> should return false for null.
		/// </summary>
		[Fact]
		public void IsValidReturnsFalseForNull()
		{
			var attribute = new NonEmptyGuidAttribute();
			Assert.False(new NonEmptyGuidAttribute().IsValid(null));
		}
	}
}