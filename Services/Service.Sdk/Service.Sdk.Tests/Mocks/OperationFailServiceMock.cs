namespace Service.Sdk.Tests.Mocks
{
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Models;

	internal class OperationFailServiceMock : ServiceBase<StringEntry, string, CreateEntry, UpdateEntry>
	{
		public OperationFailServiceMock(ILogger logger, IEntryValidator<CreateEntry, UpdateEntry, string> validator)
			: base(logger, validator)
		{
		}

		protected override Task<ClearResult> ClearEntries()
		{
			return Task.FromResult(ClearResult.InternalError);
		}

		protected override Task<IOperationResult<StringEntry, string, CreateResult>> CreateEntry(CreateEntry entry)
		{
			var result = new OperationResult<StringEntry, string, CreateResult>(CreateResult.AlreadyExists);
			return Task.FromResult(result as IOperationResult<StringEntry, string, CreateResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, DeleteResult>> DeleteEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, DeleteResult>(DeleteResult.NotFound);
			return Task.FromResult(result as IOperationResult<StringEntry, string, DeleteResult>);
		}

		protected override Task<ExistsResult> ExistsEntry(string entryId)
		{
			return Task.FromResult(ExistsResult.NotFound);
		}

		protected override Task<IOperationResult<StringEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			var result = new OperationResult<StringEntry, string, ReadResult>(ReadResult.NotFound);
			return Task.FromResult(result as IOperationResult<StringEntry, string, ReadResult>);
		}

		protected override Task<IOperationResult<StringEntry, string, UpdateResult>> UpdateEntry(UpdateEntry entry)
		{
			var result = new OperationResult<StringEntry, string, UpdateResult>(UpdateResult.NotFound);
			return Task.FromResult(result as IOperationResult<StringEntry, string, UpdateResult>);
		}
	}
}