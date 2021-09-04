namespace User.Tests.Mocks
{
	using System.Linq;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using User.Services.Models;

	public class DatabaseServiceMock : IDatabaseService<UserEntry, string>
	{
		public Task<ClearResult> Clear()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		public Task<IOperationResult<UserEntry, string, CreateResult>> Create(UserEntry entry)
		{
			var result = new OperationResult<UserEntry, string, CreateResult>(CreateResult.Created, entry);
			return Task.FromResult<IOperationResult<UserEntry, string, CreateResult>>(result);
		}

		public Task<IOperationResult<UserEntry, string, DeleteResult>> Delete(string entryId)
		{
			var result = new OperationResult<UserEntry, string, DeleteResult>(
				DeleteResult.Deleted,
				new UserEntry
				{
					Id = entryId
				});
			return Task.FromResult<IOperationResult<UserEntry, string, DeleteResult>>(result);
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

		public Task<IOperationResult<UserEntry, string, ReadResult>> Read(string entryId)
		{
			var result = new OperationResult<UserEntry, string, ReadResult>(
				ReadResult.Read,
				new UserEntry
				{
					Id = entryId
				});
			return Task.FromResult<IOperationResult<UserEntry, string, ReadResult>>(result);
		}

		public Task<IOperationResult<UserEntry, string, UpdateResult>> Update(UserEntry entry)
		{
			var result = new OperationResult<UserEntry, string, UpdateResult>(UpdateResult.Updated, entry);
			return Task.FromResult<IOperationResult<UserEntry, string, UpdateResult>>(result);
		}
	}
}