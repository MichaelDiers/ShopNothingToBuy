namespace Services.Crud.User.Tests.Mocks
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Crud.Application;
	using Service.Sdk.Contracts.Crud.Base;

	internal class ApplicationServiceMock : IApplicationService
	{
		private readonly IEnumerable<IOperationResult<ApplicationEntry, string, ReadResult>> applicationReadResult;

		public ApplicationServiceMock(
			IEnumerable<IOperationResult<ApplicationEntry, string, ReadResult>> applicationReadResult)
		{
			this.applicationReadResult = applicationReadResult;
		}

		public Task<ClearResult> Clear()
		{
			throw new NotImplementedException();
		}

		public Task<IOperationResult<ApplicationEntry, string, CreateResult>> Create(CreateApplicationEntry entry)
		{
			throw new NotImplementedException();
		}

		public Task<IOperationResult<ApplicationEntry, string, CreateResult>> Create(string json)
		{
			throw new NotImplementedException();
		}

		public Task<IOperationResult<ApplicationEntry, string, DeleteResult>> Delete(string entryId)
		{
			throw new NotImplementedException();
		}

		public Task<ExistsResult> Exists(string entryId)
		{
			throw new NotImplementedException();
		}

		public Task<IOperationListResult<string, ListResult>> List()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<IOperationResult<ApplicationEntry, string, ReadResult>>> Read(IEnumerable<string> entryIds)
		{
			return Task.FromResult(this.applicationReadResult);
		}

		public Task<IOperationResult<ApplicationEntry, string, ReadResult>> Read(string entryId)
		{
			throw new NotImplementedException();
		}

		public Task<IOperationResult<ApplicationEntry, string, UpdateResult>> Update(UpdateApplicationEntry entry)
		{
			throw new NotImplementedException();
		}

		public Task<IOperationResult<ApplicationEntry, string, UpdateResult>> Update(string json)
		{
			throw new NotImplementedException();
		}
	}
}