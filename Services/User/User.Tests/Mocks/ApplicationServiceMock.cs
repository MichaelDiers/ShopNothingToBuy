namespace User.Tests.Mocks
{
	using System.Linq;
	using System.Threading.Tasks;
	using Application.Contracts;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;

	internal class ApplicationServiceMock : IApplicationService
	{
		public Task<ClearResult> Clear()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		public Task<IOperationResult<ApplicationEntry, string, CreateResult>> Create(CreateApplicationEntry entry)
		{
			return Task.FromResult<IOperationResult<ApplicationEntry, string, CreateResult>>(
				new OperationResult<ApplicationEntry, string, CreateResult>(CreateResult.Created, new ApplicationEntry(entry)));
		}

		public Task<IOperationResult<ApplicationEntry, string, DeleteResult>> Delete(string entryId)
		{
			return Task.FromResult<IOperationResult<ApplicationEntry, string, DeleteResult>>(
				new OperationResult<ApplicationEntry, string, DeleteResult>(
					DeleteResult.Deleted,
					new ApplicationEntry(entryId, "my app name", Roles.All)));
		}

		public Task<ExistsResult> Exists(string entryId)
		{
			return Task.FromResult(ExistsResult.Exists);
		}

		public Task<IOperationListResult<string, ListResult>> List()
		{
			return Task.FromResult<IOperationListResult<string, ListResult>>(
				new OperationListResult<string, ListResult>(ListResult.Completed, Enumerable.Empty<string>()));
		}

		public Task<IOperationResult<ApplicationEntry, string, ReadResult>> Read(string entryId)
		{
			return Task.FromResult<IOperationResult<ApplicationEntry, string, ReadResult>>(
				new OperationResult<ApplicationEntry, string, ReadResult>(
					ReadResult.Read,
					new ApplicationEntry(entryId, "my app name", Roles.All)));
		}

		public Task<IOperationResult<ApplicationEntry, string, UpdateResult>> Update(UpdateApplicationEntry entry)
		{
			return Task.FromResult<IOperationResult<ApplicationEntry, string, UpdateResult>>(
				new OperationResult<ApplicationEntry, string, UpdateResult>(UpdateResult.Updated, new ApplicationEntry(entry)));
		}
	}
}