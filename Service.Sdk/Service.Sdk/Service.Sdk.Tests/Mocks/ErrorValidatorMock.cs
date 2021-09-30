namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Tests.Models;

	internal class ErrorValidatorMock : IExtendedEntryValidator
	{
		public Task<bool> ValidateCreateEntry(CreateEntry entry)
		{
			this.ValidateCreateEntryCallCount += 1;
			throw new Exception(nameof(this.ValidateCreateEntry));
		}

		public Task<bool> ValidateEntryId(string entryId)
		{
			this.ValidateEntryIdCallCount += 1;
			throw new Exception(nameof(this.ValidateEntryId));
		}

		public Task<bool> ValidateUpdateEntry(UpdateEntry entry)
		{
			this.ValidateUpdateEntryCallCount += 1;
			throw new Exception(nameof(this.ValidateUpdateEntry));
		}

		public int ValidateCreateEntryCallCount { get; private set; }

		public int ValidateEntryIdCallCount { get; private set; }

		public int ValidateUpdateEntryCallCount { get; private set; }
	}
}