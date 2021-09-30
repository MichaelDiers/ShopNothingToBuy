namespace Service.Sdk.Database.MongoDatabase.Tests.Mocks
{
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Database.MongoDatabase.Tests.Models;

	internal class EntryValidatorMock : IEntryValidator<Entry, Entry, string>
	{
		public Task<bool> ValidateCreateEntry(Entry entry)
		{
			return Task.FromResult(true);
		}

		public Task<bool> ValidateEntryId(string entryId)
		{
			return Task.FromResult(true);
		}

		public Task<bool> ValidateUpdateEntry(Entry entry)
		{
			return Task.FromResult(true);
		}
	}
}