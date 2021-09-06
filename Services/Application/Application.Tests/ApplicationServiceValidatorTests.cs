namespace Application.Tests
{
	using System;
	using Application.Contracts;
	using Application.Services;
	using Xunit;

	public class ApplicationServiceValidatorTests
	{
		[Fact]
		public async void ValidateCreateEntry_ShouldReturnFalseIfEntryIsNull()
		{
			Assert.False(await new ApplicationServiceValidator().ValidateCreateEntry(null));
		}

		[Fact]
		public async void ValidateCreateEntry_ShouldSucceed()
		{
			Assert.True(
				await new ApplicationServiceValidator().ValidateCreateEntry(
					new CreateApplicationEntry
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
		public async void ValidateEntryId_ShouldAcceptOnlyAGuid(string id, bool expectedResult)
		{
			var validator = new ApplicationServiceValidator();
			Assert.Equal(await validator.ValidateEntryId(id), expectedResult);
		}

		[Theory]
		[InlineData(null, "name", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", null, false)]
		[InlineData("", "name", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", "", false)]
		[InlineData("invalid", "name", false)]
		[InlineData("44de1638-3e90-4093-8e20-64d09572cbec", "name", true)]
		[InlineData("549f40dd-65a2-4015-9e91-86a374c7a14c", "name", true)]
		public async void ValidateUpdateEntry(string id, string name, bool expectedResult)
		{
			var validator = new ApplicationServiceValidator();
			var entry = new UpdateApplicationEntry
			{
				Id = id,
				Name = name
			};

			Assert.Equal(await validator.ValidateUpdateEntry(entry), expectedResult);
		}

		[Fact]
		public async void ValidateUpdateEntry_ShouldIfUpdateEntryIsNull()
		{
			Assert.False(await new ApplicationServiceValidator().ValidateUpdateEntry(null));
		}
	}
}