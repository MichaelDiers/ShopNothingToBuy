namespace Services.Crud.User.Tests
{
	using global::Services.Crud.User.Services;
	using Service.Sdk.Contracts.Crud.Application;
	using Service.Sdk.Contracts.Crud.User;
	using Xunit;

	public class UserServiceValidatorTests
	{
		[Theory]
		[InlineData(
			null,
			null,
			Roles.Admin,
			false)]
		[InlineData(
			"",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"a",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"aa",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"aaa",
			"a",
			Roles.Admin,
			true)]
		[InlineData(
			"aaa",
			"a",
			Roles.None,
			false)]
		[InlineData(
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"a",
			Roles.Admin,
			true)]
		[InlineData(
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"",
			Roles.Admin,
			false)]
		public async void ValidateCreateEntry(
			string id,
			string applicationId,
			Roles roles,
			bool expectedResult)
		{
			Assert.Equal(
				await new UserServiceValidator().ValidateCreateEntry(
					new CreateUserEntry
					{
						Id = id,
						Applications = new[]
						{
							new UserApplicationEntry(applicationId, roles)
						}
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
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
		public async void ValidateEntryId(string id, bool expectedResult)
		{
			var validator = new UserServiceValidator();
			Assert.Equal(await validator.ValidateEntryId(id), expectedResult);
		}

		[Theory]
		[InlineData(
			null,
			null,
			Roles.Admin,
			false)]
		[InlineData(
			"",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"a",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"aa",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"aaa",
			"a",
			Roles.Admin,
			true)]
		[InlineData(
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"a",
			Roles.Admin,
			true)]
		[InlineData(
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"a",
			Roles.Admin,
			false)]
		[InlineData(
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"",
			Roles.Admin,
			false)]
		public async void ValidateUpdateEntry(
			string id,
			string applicationId,
			Roles roles,
			bool expectedResult)
		{
			var validator = new UserServiceValidator();
			var entry = new UpdateUserEntry
			{
				Id = id,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId, roles)
				}
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