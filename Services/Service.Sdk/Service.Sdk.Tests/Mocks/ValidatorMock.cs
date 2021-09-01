namespace Service.Sdk.Tests.Mocks
{
	using System.Threading.Tasks;
	using Service.Sdk.Tests.Models;

	internal class ValidatorMock : IExtendedEntryValidator
	{
		private readonly bool createEntryResult;
		private readonly bool entryIdResult;
		private readonly bool updateEntryResult;

		public ValidatorMock()
			: this(true, true, true)
		{
		}

		public ValidatorMock(bool createEntryResult, bool entryIdResult, bool updateEntryResult)
		{
			this.createEntryResult = createEntryResult;
			this.entryIdResult = entryIdResult;
			this.updateEntryResult = updateEntryResult;
		}

		public Task<bool> ValidateCreateEntry(CreateEntry entry)
		{
			this.ValidateCreateEntryCallCount += 1;
			return Task.FromResult(this.createEntryResult);
		}

		public Task<bool> ValidateEntryId(string entryId)
		{
			this.ValidateEntryIdCallCount += 1;
			return Task.FromResult(this.entryIdResult);
		}

		public Task<bool> ValidateUpdateEntry(UpdateEntry entry)
		{
			this.ValidateUpdateEntryCallCount += 1;
			return Task.FromResult(this.updateEntryResult);
		}

		public int ValidateCreateEntryCallCount { get; private set; }

		public int ValidateEntryIdCallCount { get; private set; }

		public int ValidateUpdateEntryCallCount { get; private set; }
	}
}