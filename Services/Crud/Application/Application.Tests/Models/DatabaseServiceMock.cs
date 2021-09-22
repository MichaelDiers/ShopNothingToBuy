namespace Application.Tests.Models
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Application.Contracts;
	using Service.Contracts.Crud.Base;
	using Service.Contracts.Crud.Database;
	using Service.Sdk.Services;

	internal class DatabaseServiceMock : IDatabaseService<ApplicationEntry, string>
	{
		private readonly IDictionary<string, ApplicationEntry> database = new Dictionary<string, ApplicationEntry>();

		public Task<ClearResult> Clear()
		{
			this.database.Clear();
			return Task.FromResult(ClearResult.Cleared);
		}

		public Task<IOperationResult<ApplicationEntry, string, CreateResult>> Create(ApplicationEntry entry)
		{
			if (entry.Id.ToUpper() != entry.Id)
			{
				throw new ArgumentException($"Invalid id: {entry.Id}");
			}

			IOperationResult<ApplicationEntry, string, CreateResult> result;
			if (this.database.ContainsKey(entry.Id))
			{
				result = new OperationResult<ApplicationEntry, string, CreateResult>(CreateResult.AlreadyExists);
			}
			else
			{
				this.database.Add(entry.Id, entry);
				result = new OperationResult<ApplicationEntry, string, CreateResult>(CreateResult.Created, entry);
			}

			return Task.FromResult(result);
		}

		public Task<IOperationResult<ApplicationEntry, string, DeleteResult>> Delete(string entryId)
		{
			if (entryId.ToUpper() != entryId)
			{
				throw new ArgumentException($"Invalid id: {entryId}");
			}

			IOperationResult<ApplicationEntry, string, DeleteResult> result;
			if (this.database.Remove(entryId, out var entry))
			{
				result = new OperationResult<ApplicationEntry, string, DeleteResult>(DeleteResult.Deleted, entry);
			}
			else
			{
				result = new OperationResult<ApplicationEntry, string, DeleteResult>(DeleteResult.NotFound);
			}

			return Task.FromResult(result);
		}

		public Task<ExistsResult> Exists(string entryId)
		{
			if (entryId.ToUpper() != entryId)
			{
				throw new ArgumentException($"Invalid id: {entryId}");
			}

			return Task.FromResult(this.database.ContainsKey(entryId) ? ExistsResult.Exists : ExistsResult.NotFound);
		}

		public Task<IOperationListResult<string, ListResult>> List()
		{
			var result = new OperationListResult<string, ListResult>(ListResult.Completed, this.database.Keys);
			return Task.FromResult<IOperationListResult<string, ListResult>>(result);
		}

		public Task<IEnumerable<IOperationResult<ApplicationEntry, string, ReadResult>>> Read(IEnumerable<string> entryIds)
		{
			throw new NotImplementedException();
		}

		public Task<IOperationResult<ApplicationEntry, string, ReadResult>> Read(string entryId)
		{
			if (entryId.ToUpper() != entryId)
			{
				throw new ArgumentException($"Invalid id: {entryId}");
			}

			IOperationResult<ApplicationEntry, string, ReadResult> result;
			if (this.database.TryGetValue(entryId, out var entry))
			{
				result = new OperationResult<ApplicationEntry, string, ReadResult>(ReadResult.Read, entry);
			}
			else
			{
				result = new OperationResult<ApplicationEntry, string, ReadResult>(ReadResult.NotFound);
			}

			return Task.FromResult(result);
		}

		public Task<IOperationResult<ApplicationEntry, string, UpdateResult>> Update(ApplicationEntry entry)
		{
			if (entry.Id.ToUpper() != entry.Id)
			{
				throw new ArgumentException($"Invalid id: {entry.Id}");
			}

			IOperationResult<ApplicationEntry, string, UpdateResult> result;
			if (this.database.ContainsKey(entry.Id))
			{
				this.database[entry.Id] = entry;
				result = new OperationResult<ApplicationEntry, string, UpdateResult>(UpdateResult.Updated, entry);
			}
			else
			{
				result = new OperationResult<ApplicationEntry, string, UpdateResult>(UpdateResult.NotFound);
			}

			return Task.FromResult(result);
		}
	}
}