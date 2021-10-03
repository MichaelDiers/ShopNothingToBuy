namespace Service.Sdk.Tests.Mocks
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Services;

	/// <summary>
	///   In-memory database implementation as a <see cref="DatabaseService{TEntry,TEntryId}" />.
	/// </summary>
	/// <typeparam name="TEntry">The type of the database entries.</typeparam>
	/// <typeparam name="TEntryId">The type of the database entry id.</typeparam>
	public class InMemoryCrudServiceMock<TEntry, TEntryId> : DatabaseService<TEntry, TEntryId>
		where TEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   In-memory database.
		/// </summary>
		private readonly IDictionary<TEntryId, TEntry> database = new Dictionary<TEntryId, TEntry>();

		/// <summary>
		///   Creates a new instance of <see cref="InMemoryCrudServiceMock{TEntry,TEntryId}" />.
		/// </summary>
		/// <param name="logger">The logger used for error messages.</param>
		/// <param name="validator">The validator for input data.</param>
		public InMemoryCrudServiceMock(
			ILogger logger,
			IEntryValidator<TEntry, TEntry, TEntryId> validator)
			: base(logger, validator)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="InMemoryCrudServiceMock{TEntry,TEntryId}" />.
		/// </summary>
		public InMemoryCrudServiceMock()
			: this(new LoggerMock(), new EntryValidatorMock<TEntry, TEntry, TEntryId>())
		{
		}

		/// <summary>
		///   Delete all known entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		protected override Task<ClearResult> ClearEntries()
		{
			this.database.Clear();
			return Task.FromResult(ClearResult.Cleared);
		}

		/// <summary>
		///   Create a new entry without previous existence check.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected override Task<IOperationResult<TEntry, TEntryId, CreateResult>> CreateNewEntry(TEntry entry)
		{
			this.database.Add(entry.Id, entry);
			return Task.FromResult<IOperationResult<TEntry, TEntryId, CreateResult>>(
				new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.Created, entry));
		}

		/// <summary>
		///   Delete an entry by its id without a previous existence check.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />.
		/// </returns>
		protected override Task<IOperationResult<TEntry, TEntryId, DeleteResult>> DeleteExistingEntry(TEntryId entryId)
		{
			var deleted = this.database.Remove(entryId, out var entry);
			return deleted
				? Task.FromResult<IOperationResult<TEntry, TEntryId, DeleteResult>>(
					new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.Deleted, entry))
				: Task.FromResult<IOperationResult<TEntry, TEntryId, DeleteResult>>(
					new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.Deleted));
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		protected override Task<ExistsResult> ExistsEntry(TEntryId entryId)
		{
			return this.database.ContainsKey(entryId)
				? Task.FromResult(ExistsResult.Exists)
				: Task.FromResult(ExistsResult.NotFound);
		}

		/// <summary>
		///   List the ids of all entries.
		/// </summary>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationListResult{T,TOperationResult}" />
		///   that contains the <see cref="ListResult" />.
		/// </returns>
		protected override Task<IOperationListResult<TEntryId, ListResult>> ListEntries()
		{
			return Task.FromResult<IOperationListResult<TEntryId, ListResult>>(
				new OperationListResult<TEntryId, ListResult>(ListResult.Completed, this.database.Keys));
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, ReadResult>> ReadEntry(TEntryId entryId)
		{
			await Task.CompletedTask;
			if (this.database.ContainsKey(entryId))
			{
				return new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.Read, this.database[entryId]);
			}

			return new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.NotFound);
		}

		/// <summary>
		///   Update an entry without a previous existence check.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, UpdateResult>> UpdateExistingEntry(TEntry entry)
		{
			await Task.CompletedTask;
			this.database[entry.Id] = entry;
			return new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.Updated, entry);
		}
	}
}