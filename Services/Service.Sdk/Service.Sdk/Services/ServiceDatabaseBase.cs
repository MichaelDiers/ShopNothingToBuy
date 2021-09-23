namespace Service.Sdk.Services
{
	using System;
	using System.Threading.Tasks;
	using Service.Contracts.Business.Log;
	using Service.Contracts.Crud.Base;
	using Service.Contracts.Crud.Database;

	/// <summary>
	///   Base class for services using a database.
	/// </summary>
	/// <typeparam name="TEntry">The type of the object to process.</typeparam>
	/// <typeparam name="TEntryId">The type of the id of an entry.</typeparam>
	/// <typeparam name="TCreateEntry">The type of an entry in create operation context.</typeparam>
	/// <typeparam name="TUpdateEntry">The type of an entry in update operation context.</typeparam>
	public abstract class
		ServiceDatabaseBase<TEntry, TEntryId, TCreateEntry, TUpdateEntry>
		: ServiceBase<TEntry, TEntryId, TCreateEntry, TUpdateEntry>
		where TEntry : class, IEntry<TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Create a new instance of <see cref="ServiceDatabaseBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}" />.
		/// </summary>
		/// <param name="logger">The error logger of the application.</param>
		/// <param name="validator">The validator for input data of the application.</param>
		/// <param name="databaseService">Access to the database.</param>
		protected ServiceDatabaseBase(
			ILogger logger,
			IEntryValidator<TCreateEntry, TUpdateEntry, TEntryId> validator,
			IDatabaseService<TEntry, TEntryId> databaseService)
			: base(logger, validator)
		{
			this.DatabaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
		}

		/// <summary>
		///   Gets access to the database.
		/// </summary>
		protected IDatabaseService<TEntry, TEntryId> DatabaseService { get; }

		/// <summary>
		///   Delete all known entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		protected override async Task<ClearResult> ClearEntries()
		{
			return await this.DatabaseService.Clear();
		}

		/// <summary>
		///   Delete an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, DeleteResult>> DeleteEntry(TEntryId entryId)
		{
			return await this.DatabaseService.Delete(entryId);
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		protected override async Task<ExistsResult> ExistsEntry(TEntryId entryId)
		{
			return await this.DatabaseService.Exists(entryId);
		}

		/// <summary>
		///   List the ids of all entries.
		/// </summary>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationListResult{T,TOperationResult}" />
		///   that contains the <see cref="ListResult" />.
		/// </returns>
		protected override async Task<IOperationListResult<TEntryId, ListResult>> ListEntries()
		{
			return await this.DatabaseService.List();
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
			return await this.DatabaseService.Read(entryId);
		}
	}
}