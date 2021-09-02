namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Models;

	internal class DatabaseServiceMock : DatabaseService<StringEntry, string>
	{
		private readonly bool exists;

		public DatabaseServiceMock(ILogger logger, IEntryValidator<StringEntry, StringEntry, string> validator, bool exists)
			: base(logger, validator)
		{
			this.exists = exists;
		}

		protected override Task<ClearResult> ClearEntries()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		protected override Task<IOperationResult<StringEntry, string, CreateResult>> CreateNewEntry(StringEntry entry)
		{
			var result = new OperationResult<StringEntry, string, CreateResult>(CreateResult.Created, entry);
			return Task.FromResult(result as IOperationResult<StringEntry, string, CreateResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, DeleteResult>> DeleteExistingEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, DeleteResult>(
				DeleteResult.Deleted,
				new StringEntry(entryId, Guid.NewGuid().ToString()));
			return Task.FromResult(result as IOperationResult<StringEntry, string, DeleteResult>);
		}

		protected override Task<ExistsResult> ExistsEntry(string entryId)
		{
			return Task.FromResult(this.exists ? ExistsResult.Exists : ExistsResult.NotFound);
		}

		protected override Task<IOperationListResult<string, ListResult>> ListEntries()
		{
			var result = new OperationListResult<string, ListResult>(ListResult.Completed, Enumerable.Empty<string>());
			return Task.FromResult(result as IOperationListResult<string, ListResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, ReadResult>(
				ReadResult.Read,
				new StringEntry(entryId, Guid.NewGuid().ToString()));
			return Task.FromResult(result as IOperationResult<StringEntry, string, ReadResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, UpdateResult>> UpdateExistingEntry(StringEntry entry)
		{
			var result = new OperationResult<StringEntry, string, UpdateResult>(UpdateResult.Updated, entry);
			return Task.FromResult(result as IOperationResult<StringEntry, string, UpdateResult>);
		}
	}
}