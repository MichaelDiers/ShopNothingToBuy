namespace User.Tests
{
	using System;
	using User.Services;
	using User.Services.Models;
	using Xunit;

	public class UserServiceValidatorTests
	{
		[Fact]
		public async void ValidateCreateEntry_ShouldReturnFalseIfUserIsNull()
		{
			Assert.False(await new UserServiceValidator().ValidateCreateEntry(null));
		}

		[Fact]
		public async void ValidateCreateEntry_ShouldSucceed()
		{
			Assert.True(
				await new UserServiceValidator().ValidateCreateEntry(
					new CreateUserEntry
					{
						Name = Guid.NewGuid().ToString()
					}));
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("invalid", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", true)]
		[InlineData("549f40dd-65a2-4015-9e91-86a374c7a14c", true)]
		public async void ValidateEntryId_ShouldAcceptOnlyAGuid(string userId, bool expectedResult)
		{
			var validator = new UserServiceValidator();
			Assert.Equal(await validator.ValidateEntryId(userId), expectedResult);
		}

		[Theory]
		[InlineData(null, "name", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", null, false)]
		[InlineData("", "name", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", "", false)]
		[InlineData("invalid", "name", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", "name", true)]
		[InlineData("549f40dd-65a2-4015-9e91-86a374c7a14c", "name", true)]
		public async void ValidateUpdateEntry(string userId, string name, bool expectedResult)
		{
			var validator = new UserServiceValidator();
			var entry = new UpdateUserEntry
			{
				Id = userId,
				Name = name
			};

			Assert.Equal(await validator.ValidateUpdateEntry(entry), expectedResult);
		}

		[Fact]
		public async void ValidateUpdateEntry_ShouldIfUpdateUserIsNull()
		{
			Assert.False(await new UserServiceValidator().ValidateUpdateEntry(null));
		}
	}
}