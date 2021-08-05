namespace ShopNothingToBuy.Sdk.Test.Attributes
{
	using System.ComponentModel.DataAnnotations;
	using ShopNothingToBuy.Sdk.Attributes;
	using Xunit;

	public class IsGuidAttributeTests
	{
		[Fact]
		public void IsValidationAttribute()
		{
			Assert.IsAssignableFrom<ValidationAttribute>(new IsGuidAttribute());
		}
	}
}