namespace MongoDatabase.Tests.Mocks
{
	using System.Threading.Tasks;
	using MongoDatabase.Tests.Models;
	using Service.Contracts.Crud.Base;

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