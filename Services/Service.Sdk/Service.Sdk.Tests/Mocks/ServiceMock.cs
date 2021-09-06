namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Models;

	internal class ServiceMock : ServiceBase<StringEntry, string, CreateEntry, UpdateEntry>
	{
		public ServiceMock(ILogger logger, IEntryValidator<CreateEntry, UpdateEntry, string> validator)
			: base(logger, validator)
		{
		}

		protected override Task<ClearResult> ClearEntries()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		protected override Task<IOperationResult<StringEntry, string, CreateResult>> CreateEntry(CreateEntry entry)
		{
			var result = new OperationResult<StringEntry, string, CreateResult>(CreateResult.Created, new StringEntry(entry));
			return Task.FromResult<IOperationResult<StringEntry, string, CreateResult>>(result);
		}

		protected override Task<IOperationResult<StringEntry, string, DeleteResult>> DeleteEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, DeleteResult>(
				DeleteResult.Deleted,
				new StringEntry(entryId, Guid.NewGuid().ToString()));
			return Task.FromResult<IOperationResult<StringEntry, string, DeleteResult>>(result);
		}

		protected override Task<ExistsResult> ExistsEntry(string entryId)
		{
			return Task.FromResult(ExistsResult.Exists);
		}

		protected override Task<IOperationListResult<string, ListResult>> ListEntries()
		{
			var result = new OperationListResult<string, ListResult>(
				ListResult.Completed,
				new[]
				{
					Guid.NewGuid().ToString(),
					Guid.NewGuid().ToString()
				});
			return Task.FromResult<IOperationListResult<string, ListResult>>(result);
		}

		protected override Task<IOperationResult<StringEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, ReadResult>(
				ReadResult.Read,
				new StringEntry(entryId, Guid.NewGuid().ToString()));
			return Task.FromResult<IOperationResult<StringEntry, string, ReadResult>>(result);
		}

		protected override Task<IOperationResult<StringEntry, string, UpdateResult>> UpdateEntry(UpdateEntry entry)
		{
			var result = new OperationResult<StringEntry, string, UpdateResult>(UpdateResult.Updated, new StringEntry(entry));
			return Task.FromResult<IOperationResult<StringEntry, string, UpdateResult>>(result);
		}
	}
}