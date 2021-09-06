namespace Application.Tests.Models
{
	using System.Linq;
	using System.Threading.Tasks;
	using Application.Contracts;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;

	internal class DatabaseServiceMock : IDatabaseService<ApplicationEntry, string>
	{
		public Task<ClearResult> Clear()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		public Task<IOperationResult<ApplicationEntry, string, CreateResult>> Create(ApplicationEntry entry)
		{
			var result = new OperationResult<ApplicationEntry, string, CreateResult>(CreateResult.Created, entry);
			return Task.FromResult<IOperationResult<ApplicationEntry, string, CreateResult>>(result);
		}

		public Task<IOperationResult<ApplicationEntry, string, DeleteResult>> Delete(string entryId)
		{
			var result = new OperationResult<ApplicationEntry, string, DeleteResult>(
				DeleteResult.Deleted,
				new ApplicationEntry
				{
					Id = entryId
				});
			return Task.FromResult<IOperationResult<ApplicationEntry, string, DeleteResult>>(result);
		}

		public Task<ExistsResult> Exists(string entryId)
		{
			return Task.FromResult(ExistsResult.Exists);
		}

		public Task<IOperationListResult<string, ListResult>> List()
		{
			var result = new OperationListResult<string, ListResult>(ListResult.Completed, Enumerable.Empty<string>());
			return Task.FromResult<IOperationListResult<string, ListResult>>(result);
		}

		public Task<IOperationResult<ApplicationEntry, string, ReadResult>> Read(string entryId)
		{
			var result = new OperationResult<ApplicationEntry, string, ReadResult>(
				ReadResult.Read,
				new ApplicationEntry
				{
					Id = entryId
				});
			return Task.FromResult<IOperationResult<ApplicationEntry, string, ReadResult>>(result);
		}

		public Task<IOperationResult<ApplicationEntry, string, UpdateResult>> Update(ApplicationEntry entry)
		{
			var result = new OperationResult<ApplicationEntry, string, UpdateResult>(UpdateResult.Updated, entry);
			return Task.FromResult<IOperationResult<ApplicationEntry, string, UpdateResult>>(result);
		}
	}
}