namespace User.Tests.Mocks
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using User.Services.Models;

	public class DatabaseServiceMock : IDatabaseService<UserEntry, string>
	{
		private readonly IDictionary<string, UserEntry> database = new Dictionary<string, UserEntry>();

		public Task<ClearResult> Clear()
		{
			this.database.Clear();
			return Task.FromResult(ClearResult.Cleared);
		}

		public Task<IOperationResult<UserEntry, string, CreateResult>> Create(UserEntry entry)
		{
			if (entry.Id != entry.Id.ToUpper())
			{
				throw new ArgumentException($"Invalid entry id: {entry.Id}");
			}

			this.database.Add(entry.Id, entry);
			var result = new OperationResult<UserEntry, string, CreateResult>(CreateResult.Created, entry);
			return Task.FromResult<IOperationResult<UserEntry, string, CreateResult>>(result);
		}

		public Task<IOperationResult<UserEntry, string, DeleteResult>> Delete(string entryId)
		{
			if (entryId != entryId.ToUpper())
			{
				throw new ArgumentException($"Invalid entry id: {entryId}");
			}

			IOperationResult<UserEntry, string, DeleteResult> result;
			if (this.database.Remove(entryId, out var entry))
			{
				result = new OperationResult<UserEntry, string, DeleteResult>(DeleteResult.Deleted, entry);
			}
			else
			{
				result = new OperationResult<UserEntry, string, DeleteResult>(DeleteResult.NotFound);
			}

			return Task.FromResult(result);
		}

		public Task<ExistsResult> Exists(string entryId)
		{
			if (entryId != entryId.ToUpper())
			{
				throw new ArgumentException($"Invalid entry id: {entryId}");
			}

			return Task.FromResult(this.database.ContainsKey(entryId) ? ExistsResult.Exists : ExistsResult.NotFound);
		}

		public Task<IOperationListResult<string, ListResult>> List()
		{
			var result = new OperationListResult<string, ListResult>(ListResult.Completed, this.database.Keys);
			return Task.FromResult<IOperationListResult<string, ListResult>>(result);
		}

		public Task<IOperationResult<UserEntry, string, ReadResult>> Read(string entryId)
		{
			if (entryId != entryId.ToUpper())
			{
				throw new ArgumentException($"Invalid entry id: {entryId}");
			}

			IOperationResult<UserEntry, string, ReadResult> result;
			if (this.database.TryGetValue(entryId, out var entry))
			{
				result = new OperationResult<UserEntry, string, ReadResult>(ReadResult.Read, entry);
			}
			else
			{
				result = new OperationResult<UserEntry, string, ReadResult>(ReadResult.NotFound);
			}

			return Task.FromResult(result);
		}

		public Task<IOperationResult<UserEntry, string, UpdateResult>> Update(UserEntry entry)
		{
			if (entry.Id != entry.Id.ToUpper())
			{
				throw new ArgumentException($"Invalid entry id: {entry.Id}");
			}

			IOperationResult<UserEntry, string, UpdateResult> result;
			if (this.database.ContainsKey(entry.Id))
			{
				this.database[entry.Id] = entry;
				result = new OperationResult<UserEntry, string, UpdateResult>(UpdateResult.Updated, entry);
			}
			else
			{
				result = new OperationResult<UserEntry, string, UpdateResult>(UpdateResult.NotFound);
			}

			return Task.FromResult(result);
		}
	}
}