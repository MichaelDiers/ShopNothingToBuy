namespace Service.Sdk.Services
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Contracts.Crud.Database;

	/// <summary>
	///   Base class for database services.
	/// </summary>
	/// <typeparam name="TEntry">The type of the database entries.</typeparam>
	/// <typeparam name="TEntryId">The type of the database entry id.</typeparam>
	public abstract class DatabaseService<TEntry, TEntryId> : ServiceBase<TEntry, TEntryId, TEntry, TEntry>,
		IDatabaseService<TEntry, TEntryId>
		where TEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Creates a new instance of <see cref="DatabaseService{TEntry,TEntryId}" />.
		/// </summary>
		/// <param name="logger">A logger for error messages.</param>
		/// <param name="validator">A validator for input messages.</param>
		protected DatabaseService(ILogger logger, IEntryValidator<TEntry, TEntry, TEntryId> validator)
			: base(logger, validator)
		{
		}

		/// <summary>
		///   Create a new entry. A previous check if the entry already exists if executed.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, CreateResult>> CreateEntry(TEntry entry)
		{
			var exists = await this.Exists(entry.Id);
			switch (exists)
			{
				case ExistsResult.Exists:
					return new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.AlreadyExists);
				case ExistsResult.NotFound:
					return await this.CreateNewEntry(entry);
				case ExistsResult.InvalidData:
					return new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.InvalidData);
				case ExistsResult.InternalError:
					return new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.InternalError);
				default:
					throw new ArgumentOutOfRangeException(nameof(exists), exists, "Unhandled value.");
			}
		}

		/// <summary>
		///   Create a new entry without previous existence check.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected abstract Task<IOperationResult<TEntry, TEntryId, CreateResult>> CreateNewEntry(TEntry entry);

		/// <summary>
		///   Delete an entry by its id including a previous existence check.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, DeleteResult>> DeleteEntry(TEntryId entryId)
		{
			var exists = await this.Exists(entryId);
			switch (exists)
			{
				case ExistsResult.Exists:
					return await this.DeleteExistingEntry(entryId);
				case ExistsResult.NotFound:
					return new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.NotFound);
				case ExistsResult.InvalidData:
					return new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.InvalidData);
				case ExistsResult.InternalError:
					return new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.InternalError);
				default:
					throw new ArgumentOutOfRangeException(nameof(exists), exists, "Unhandled value");
			}
		}

		/// <summary>
		///   Delete an entry by its id without a previous existence check.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		protected abstract Task<IOperationResult<TEntry, TEntryId, DeleteResult>> DeleteExistingEntry(TEntryId entryId);

		/// <summary>
		///   Update an entry including a previous existence check.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, UpdateResult>> UpdateEntry(TEntry entry)
		{
			var exists = await this.Exists(entry.Id);
			switch (exists)
			{
				case ExistsResult.Exists:
					return await this.UpdateExistingEntry(entry);
				case ExistsResult.NotFound:
					return new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.NotFound);
				case ExistsResult.InvalidData:
					return new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.InvalidData);
				case ExistsResult.InternalError:
					return new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.InternalError);
				default:
					throw new ArgumentOutOfRangeException(nameof(exists), exists, "Unhandled value");
			}
		}

		/// <summary>
		///   Update an entry without a previous existence check.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected abstract Task<IOperationResult<TEntry, TEntryId, UpdateResult>> UpdateExistingEntry(TEntry entry);
	}
}