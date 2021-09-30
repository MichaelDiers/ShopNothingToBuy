namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Models;

	internal class ErrorServiceMock : ServiceBase<StringEntry, string, CreateEntry, UpdateEntry>
	{
		public ErrorServiceMock(ILogger logger, IEntryValidator<CreateEntry, UpdateEntry, string> validator)
			: base(logger, validator)
		{
		}

		protected override Task<ClearResult> ClearEntries()
		{
			throw new Exception(nameof(this.Clear));
		}

		protected override Task<IOperationResult<StringEntry, string, CreateResult>> CreateEntry(CreateEntry entry)
		{
			throw new Exception(nameof(this.Create));
		}

		protected override Task<IOperationResult<StringEntry, string, DeleteResult>> DeleteEntry(string entryId)
		{
			throw new Exception(nameof(this.Delete));
		}

		protected override Task<ExistsResult> ExistsEntry(string entryId)
		{
			throw new Exception(nameof(this.Exists));
		}

		protected override Task<IOperationListResult<string, ListResult>> ListEntries()
		{
			throw new Exception(nameof(this.List));
		}

		protected override Task<IOperationResult<StringEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			throw new Exception(nameof(this.Read));
		}

		protected override Task<IOperationResult<StringEntry, string, UpdateResult>> UpdateEntry(UpdateEntry entry)
		{
			throw new Exception(nameof(this.Update));
		}
	}
}