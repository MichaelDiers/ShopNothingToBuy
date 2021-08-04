namespace AuthApi.Tests.Contracts
{
	using System;
	using System.Linq;
	using AuthApi.Contracts;
	using Xunit;

	public class AuthRoleTests
	{
		[Fact]
		public void AllShouldBeDefined()
		{
			Assert.True(Enum.IsDefined(typeof(AuthRole), "All"));
		}

		[Fact]
		public void AllShouldBeDefinedAsCombinationOfAllValues()
		{
			var value = Enum.GetNames(typeof(AuthRole))
				.Select(Enum.Parse<AuthRole>)
				.Where(e => e != AuthRole.All)
				.Aggregate(AuthRole.None, (current, enumValue) => current | enumValue);

			Assert.Equal(AuthRole.All, value);
		}

		[Fact]
		public void FlagsShouldBeUnique()
		{
			var names = Enum.GetNames(typeof(AuthRole));
			Assert.Equal(names.Length, names.Select(name => (int) Enum.Parse<AuthRole>(name)).Distinct().Count());
		}

		[Fact]
		public void NoneShouldBeDefined()
		{
			Assert.True(Enum.IsDefined(typeof(AuthRole), "None"));
		}

		[Fact]
		public void NoneShouldBeZero()
		{
			Assert.Equal(0, (int) AuthRole.None);
		}
	}
}