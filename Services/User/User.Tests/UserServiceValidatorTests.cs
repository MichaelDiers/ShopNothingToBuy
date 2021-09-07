namespace User.Tests
{
	using User.Services;
	using User.Services.Models;
	using Xunit;

	public class UserServiceValidatorTests
	{
		[Theory]
		[InlineData(null, null, false)]
		[InlineData("", "a", false)]
		[InlineData("a", "a", false)]
		[InlineData("aa", "a", false)]
		[InlineData("aaa", "a", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaa", "a", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaa", "a", false)]
		[InlineData("aaaaaaaaaaaaaaaaaaaa", "", false)]
		public async void ValidateCreateEntry(string id, string applicationId, bool expectedResult)
		{
			Assert.Equal(
				await new UserServiceValidator().ValidateCreateEntry(
					new CreateUserEntry
					{
						Id = id,
						ApplicationId = applicationId
					}),
				expectedResult);
		}

		[Fact]
		public async void ValidateCreateEntry_ShouldReturnFalseIfUserIsNull()
		{
			Assert.False(await new UserServiceValidator().ValidateCreateEntry(null));
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("a", false)]
		[InlineData("aa", false)]
		[InlineData("aaa", true)]
		[InlineData("aaaa", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaa", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaa", false)]
		public async void ValidateEntryId(string id, bool expectedResult)
		{
			var validator = new UserServiceValidator();
			Assert.Equal(await validator.ValidateEntryId(id), expectedResult);
		}

		[Theory]
		[InlineData(null, null, false)]
		[InlineData("", "a", false)]
		[InlineData("a", "a", false)]
		[InlineData("aa", "a", false)]
		[InlineData("aaa", "a", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaa", "a", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaa", "a", false)]
		[InlineData("aaaaaaaaaaaaaaaaaaaa", "", false)]
		public async void ValidateUpdateEntry(
			string id,
			string applicationId,
			bool expectedResult)
		{
			var validator = new UserServiceValidator();
			var entry = new UpdateUserEntry
			{
				Id = id,
				ApplicationId = applicationId
			};

			Assert.Equal(await validator.ValidateUpdateEntry(entry), expectedResult);
		}

		[Fact]
		public async void ValidateUpdateEntry_ShouldFailIfUpdateUserIsNull()
		{
			Assert.False(await new UserServiceValidator().ValidateUpdateEntry(null));
		}
	}
}