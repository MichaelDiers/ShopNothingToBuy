namespace Service.Sdk.Tests.Mocks
{
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Tests.Models;

	internal class DatabaseValidatorMock : IEntryValidator<StringEntry, StringEntry, string>
	{
		private readonly bool validateCreateEntry;
		private readonly bool validateEntryId;
		private readonly bool validateUpdateEntry;

		public DatabaseValidatorMock()
			: this(true, true, true)
		{
		}

		public DatabaseValidatorMock(bool validateCreateEntry, bool validateEntryId, bool validateUpdateEntry)
		{
			this.validateCreateEntry = validateCreateEntry;
			this.validateEntryId = validateEntryId;
			this.validateUpdateEntry = validateUpdateEntry;
		}

		public Task<bool> ValidateCreateEntry(StringEntry entry)
		{
			return Task.FromResult(this.validateCreateEntry);
		}

		public Task<bool> ValidateEntryId(string entryId)
		{
			return Task.FromResult(this.validateEntryId);
		}

		public Task<bool> ValidateUpdateEntry(StringEntry entry)
		{
			return Task.FromResult(this.validateUpdateEntry);
		}
	}
}