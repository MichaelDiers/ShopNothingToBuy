namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
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
			return Task.FromResult(result as IOperationResult<StringEntry, string, CreateResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, DeleteResult>> DeleteEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, DeleteResult>(
				DeleteResult.Deleted,
				new StringEntry(entryId, Guid.NewGuid().ToString()));
			return Task.FromResult(result as IOperationResult<StringEntry, string, DeleteResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, ReadResult>(
				ReadResult.Read,
				new StringEntry(entryId, Guid.NewGuid().ToString()));
			return Task.FromResult(result as IOperationResult<StringEntry, string, ReadResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, UpdateResult>> UpdateEntry(UpdateEntry entry)
		{
			var result = new OperationResult<StringEntry, string, UpdateResult>(UpdateResult.Updated, new StringEntry(entry));
			return Task.FromResult(result as IOperationResult<StringEntry, string, UpdateResult>);
		}
	}
}