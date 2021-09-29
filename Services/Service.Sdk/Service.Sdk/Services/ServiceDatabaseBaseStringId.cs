namespace Application.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Service.Contracts.Business.Log;
    using Service.Contracts.Crud.Base;
    using Service.Contracts.Crud.Database;
    using Service.Sdk.Services;

    /// <summary>
    ///   Base class for services using a database with string ids.
    /// </summary>
    /// <typeparam name="TEntry">The type of the object to process.</typeparam>
    /// <typeparam name="TCreateEntry">The type of an entry in create operation context.</typeparam>
    /// <typeparam name="TUpdateEntry">The type of an entry in update operation context.</typeparam>
    public abstract class ServiceDatabaseBaseStringId<TEntry, TCreateEntry, TUpdateEntry> : ServiceDatabaseBase<TEntry, string,
		TCreateEntry,
		TUpdateEntry>
		where TEntry : class, IEntry<string>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<string>
	{
		/// <summary>
		///   Creates a new instance of <see cref="ServiceDatabaseBaseStringId{TEntry, TCreateEntry, TUpdateEntry}" />.
		/// </summary>
		/// <param name="logger">The logger for error messages.</param>
		/// <param name="validator">THe validator for input data.</param>
		/// <param name="databaseService">The service for accessing the database.</param>
		protected ServiceDatabaseBaseStringId(
			ILogger logger,
			IEntryValidator<TCreateEntry, TUpdateEntry, string> validator,
			IDatabaseService<TEntry, string> databaseService)
			: base(logger, validator, databaseService)
		{
		}

		/// <summary>
		///   Delete an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		public override async Task<IOperationResult<TEntry, string, DeleteResult>> Delete(string entryId)
		{
			return await base.Delete(entryId?.ToUpper());
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		public override async Task<ExistsResult> Exists(string entryId)
		{
			return await base.Exists(entryId?.ToUpper());
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		public override async Task<IOperationResult<TEntry, string, ReadResult>> Read(string entryId)
		{
			return await base.Read(entryId?.ToUpper());
		}

		/// <summary>
		///   Read entries by its id.
		/// </summary>
		/// <param name="entryIds">The ids to be read.</param>
		/// <returns>A Task whose result contains the read results.</returns>
		public override Task<IEnumerable<IOperationResult<TEntry, string, ReadResult>>> Read(
			IEnumerable<string> entryIds)
		{
			var uppercaseEntryIds = entryIds?.Select(entryId => entryId?.ToUpper()).ToArray();
			return base.Read(uppercaseEntryIds);
		}
	}
}