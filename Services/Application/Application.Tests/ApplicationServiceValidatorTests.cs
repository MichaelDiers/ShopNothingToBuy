namespace Application.Tests
{
	using Application.Contracts;
	using Application.Services;
	using Xunit;

	public class ApplicationServiceValidatorTests
	{
		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("a", false)]
		[InlineData("aa", false)]
		[InlineData("aaa", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)]
		public async void ValidateCreateEntry(string applicationId, bool expectedResult)
		{
			Assert.Equal(
				await new ApplicationServiceValidator().ValidateCreateEntry(
					new CreateApplicationEntry
					{
						Id = applicationId
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
		[InlineData(null, "originalid", false)]
		[InlineData("", "", false)]
		[InlineData("A", "a", false)]
		[InlineData("AA", "aa", false)]
		[InlineData("AAA", "aaa", true)]
		[InlineData(
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			true)]
		[InlineData(
			"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			false)]
		public async void ValidateUpdateEntry(string id, string originalId, bool expectedResult)
		{
			Assert.Equal(
				expectedResult,
				await new ApplicationServiceValidator().ValidateUpdateEntry(
					new UpdateApplicationEntry
					{
						Id = id,
						OriginalId = originalId
					}));
		}

		[Fact]
		public async void ValidateUpdateEntry_ShouldIfUpdateEntryIsNull()
		{
			Assert.False(await new ApplicationServiceValidator().ValidateUpdateEntry(null));
		}
	}
}