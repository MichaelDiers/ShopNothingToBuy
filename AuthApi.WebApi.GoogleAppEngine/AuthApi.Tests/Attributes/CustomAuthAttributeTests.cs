namespace AuthApi.Tests.Attributes
{
	using System;
	using AuthApi.Attributes;
	using AuthApi.Contracts;
	using Xunit;

	public class CustomAuthAttributeTests
	{
		[Fact]
		public void CheckMappingsComplete()
		{
			foreach (var name in Enum.GetNames(typeof(AuthRole)))
			{
				var role = Enum.Parse<AuthRole>(name);
				if (role != AuthRole.None)
				{
					var attribute = new CustomAuthAttribute(role);
					Assert.IsType<CustomAuthAttribute>(attribute);
				}
			}
		}

		[Fact]
		public void ConstructorForNonRefreshTokens()
		{
			var attribute = new CustomAuthAttribute(AuthRole.All);
			Assert.IsType<CustomAuthAttribute>(attribute);
		}

		[Fact]
		public void ConstructorForRefreshTokens()
		{
			var attribute = new CustomAuthAttribute(true, AuthRole.All);
			Assert.IsType<CustomAuthAttribute>(attribute);
		}

		[Fact]
		public void ConstructorShouldThrowExceptionForRoleNone()
		{
			Assert.Throws<ArgumentException>(() => new CustomAuthAttribute(AuthRole.None));
		}
	}
}