namespace Service.Sdk.Tests.Mocks
{
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Tests.Models;

	internal interface IExtendedEntryValidator : IEntryValidator<CreateEntry, UpdateEntry, string>
	{
		int ValidateCreateEntryCallCount { get; }

		int ValidateEntryIdCallCount { get; }

		int ValidateUpdateEntryCallCount { get; }
	}
}