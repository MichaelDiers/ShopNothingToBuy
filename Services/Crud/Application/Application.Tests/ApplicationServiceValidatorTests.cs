namespace Application.Tests
{
	using Application.Services;
	using Service.Sdk.Contracts.Crud.Application;
	using Xunit;

	public class ApplicationServiceValidatorTests
	{
		[Theory]
		[InlineData(null, Roles.Admin, false)]
		[InlineData("", Roles.Admin, false)]
		[InlineData("a", Roles.Admin, false)]
		[InlineData("aa", Roles.Admin, false)]
		[InlineData("aaa", Roles.Admin, true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Roles.Admin, true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Roles.Admin, false)]
		[InlineData("aaa", Roles.None, false)]
		public async void ValidateCreateEntry(string applicationId, Roles roles, bool expectedResult)
		{
			Assert.Equal(
				await new ApplicationServiceValidator().ValidateCreateEntry(
					new CreateApplicationEntry
					{
						Id = applicationId,
						Roles = roles
					}),
				expectedResult);
		}

		[Fact]
		public async void ValidateCreateEntry_ShouldReturnFalseIfEntryIsNull()
		{
			Assert.False(await new ApplicationServiceValidator().ValidateCreateEntry(null));
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("a", false)]
		[InlineData("aa", false)]
		[InlineData("aaa", false)]
		[InlineData("AAA", true)]
		[InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
		[InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false)]
		public async void ValidateEntryId(string applicationId, bool expectedResult)
		{
			Assert.Equal(await new ApplicationServiceValidator().ValidateEntryId(applicationId), expectedResult);
		}

		[Theory]
		[InlineData(
			null,
			Roles.Admin,
			"originalid",
			false)]
		[InlineData(
			"",
			Roles.Admin,
			"",
			false)]
		[InlineData(
			"A",
			Roles.Admin,
			"a",
			false)]
		[InlineData(
			"AA",
			Roles.Admin,
			"aa",
			false)]
		[InlineData(
			"AAA",
			Roles.Admin,
			"aaa",
			true)]
		[InlineData(
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			true)]
		[InlineData(
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			false)]
		public async void ValidateUpdateEntry(
			string id,
			Roles roles,
			string originalId,
			bool expectedResult)
		{
			Assert.Equal(
				expectedResult,
				await new ApplicationServiceValidator().ValidateUpdateEntry(
					new UpdateApplicationEntry
					{
						Id = id,
						OriginalId = originalId,
						Roles = roles
					}));
		}

		[Fact]
		public async void ValidateUpdateEntry_ShouldFailIfUpdateEntryIsNull()
		{
			Assert.False(await new ApplicationServiceValidator().ValidateUpdateEntry(null));
		}
	}
}