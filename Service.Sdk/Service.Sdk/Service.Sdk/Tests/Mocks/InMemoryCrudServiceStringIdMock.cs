namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Services;

	/// <summary>
	///   In-memory database implementation as a <see cref="DatabaseService{TEntry,TEntryId}" />.
	/// </summary>
	/// <typeparam name="TEntry">The type of the database entries.</typeparam>
	public class InMemoryCrudServiceStringIdMock<TEntry> : InMemoryCrudServiceMock<TEntry, string>
		where TEntry : class, IEntry<string>
	{
		/// <summary>
		///   Creates a new instance of <see cref="InMemoryCrudServiceStringIdMock{TEntry}" />.
		/// </summary>
		/// <param name="logger">The logger used for error messages.</param>
		/// <param name="validator">The validator for input data.</param>
		public InMemoryCrudServiceStringIdMock(
			ILogger logger,
			IEntryValidator<TEntry, TEntry, string> validator)
			: base(logger, validator)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="InMemoryCrudServiceStringIdMock{TEntry}" />.
		/// </summary>
		public InMemoryCrudServiceStringIdMock()
			: this(new LoggerMock(), new EntryValidatorStringIdMock<TEntry, TEntry>())
		{
		}

		/// <summary>
		///   Create a new entry without previous existence check.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, string, CreateResult>> CreateNewEntry(TEntry entry)
		{
			if (entry == null || string.IsNullOrWhiteSpace(entry.Id) || entry.Id != entry.Id.ToUpper())
			{
				throw new NotImplementedException();
			}

			return await base.CreateNewEntry(entry);
		}

		/// <summary>
		///   Delete an entry by its id without a previous existence check.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, string, DeleteResult>> DeleteExistingEntry(string entryId)
		{
			if (string.IsNullOrWhiteSpace(entryId) || entryId != entryId.ToUpper())
			{
				throw new NotImplementedException();
			}

			return await base.DeleteExistingEntry(entryId);
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		protected override async Task<ExistsResult> ExistsEntry(string entryId)
		{
			if (string.IsNullOrWhiteSpace(entryId) || entryId != entryId.ToUpper())
			{
				throw new NotImplementedException();
			}

			return await base.ExistsEntry(entryId);
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			if (string.IsNullOrWhiteSpace(entryId) || entryId != entryId.ToUpper())
			{
				throw new NotImplementedException();
			}

			return await base.ReadEntry(entryId);
		}

		/// <summary>
		///   Update an entry without a previous existence check.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, string, UpdateResult>> UpdateExistingEntry(TEntry entry)
		{
			if (entry == null || string.IsNullOrWhiteSpace(entry.Id) || entry.Id != entry.Id.ToUpper())
			{
				throw new NotImplementedException();
			}

			return await base.UpdateExistingEntry(entry);
		}
	}
}