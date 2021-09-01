namespace User.Tests
{
	using User.Services;
	using User.Services.Models;
	using Xunit;

	public class UserValidatorTests
	{
		[Fact]
		public async void ValidateCreateEntry_ShouldReturnFalseIfUserIsNull()
		{
			Assert.False(await new UserValidator().ValidateCreateEntry(null));
		}

		[Fact]
		public async void ValidateCreateEntry_ShouldSucceed()
		{
			Assert.True(await new UserValidator().ValidateCreateEntry(new CreateUserEntry()));
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("invalid", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", true)]
		[InlineData("549f40dd-65a2-4015-9e91-86a374c7a14c", true)]
		public async void ValidateEntryId_ShouldAcceptOnlyAGuid(string userId, bool expectedResult)
		{
			var validator = new UserValidator();
			Assert.Equal(await validator.ValidateEntryId(userId), expectedResult);
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("invalid", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", true)]
		[InlineData("549f40dd-65a2-4015-9e91-86a374c7a14c", true)]
		public async void ValidateUpdateEntry(string userId, bool expectedResult)
		{
			var validator = new UserValidator();
			var entry = new UpdateUserEntry
			{
				Id = userId
			};

			Assert.Equal(await validator.ValidateUpdateEntry(entry), expectedResult);
		}

		[Fact]
		public async void ValidateUpdateEntry_ShouldIfUpdateUserIsNull()
		{
			Assert.False(await new UserValidator().ValidateUpdateEntry(null));
		}
	}
}