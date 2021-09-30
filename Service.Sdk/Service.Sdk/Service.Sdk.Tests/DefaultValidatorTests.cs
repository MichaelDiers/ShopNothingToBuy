namespace Service.Sdk.Tests
{
	using System;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="DefaultValidator{TCreateEntry,TUpdateEntry,TEntryId}" />
	/// </summary>
	public class DefaultValidatorTests
	{
		[Fact]
		public async void ValidateCreateEntry_ShouldReturnTrue()
		{
			var validator = new DefaultValidator<CreateEntry, UpdateEntry, string>();
			Assert.True(await validator.ValidateCreateEntry(new CreateEntry(10)));
		}

		[Fact]
		public async void ValidateEntryId_ShouldReturnTrue()
		{
			var validator = new DefaultValidator<CreateEntry, UpdateEntry, string>();
			Assert.True(await validator.ValidateEntryId(Guid.NewGuid().ToString()));
		}

		[Fact]
		public async void ValidateUpdateEntry_ShouldReturnTrue()
		{
			var validator = new DefaultValidator<CreateEntry, UpdateEntry, string>();
			Assert.True(await validator.ValidateUpdateEntry(new UpdateEntry(Guid.NewGuid().ToString(), "my value")));
		}
	}
}