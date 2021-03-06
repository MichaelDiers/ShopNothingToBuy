namespace Service.Sdk.Contracts.Crud.Base
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	///   Describes service operations.
	/// </summary>
	/// <typeparam name="TEntry">The type of the object to process.</typeparam>
	/// <typeparam name="TEntryId">The type of the id of an entry.</typeparam>
	/// <typeparam name="TCreateEntry">The type of an entry in create operation context.</typeparam>
	/// <typeparam name="TUpdateEntry">The type of an entry in update operation context.</typeparam>
	public interface IServiceBase<TEntry, TEntryId, in TCreateEntry, in TUpdateEntry>
		where TEntry : class, IEntry<TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Delete all known entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		Task<ClearResult> Clear();

		/// <summary>
		///   Create a new entry.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		Task<IOperationResult<TEntry, TEntryId, CreateResult>> Create(TCreateEntry entry);

		/// <summary>
		///   Create a new entry.
		/// </summary>
		/// <param name="json">A json serialized <typeparamref name="TCreateEntry" />.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		Task<IOperationResult<TEntry, TEntryId, CreateResult>> Create(string json);

		/// <summary>
		///   Delete an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		Task<IOperationResult<TEntry, TEntryId, DeleteResult>> Delete(TEntryId entryId);

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		Task<ExistsResult> Exists(TEntryId entryId);

		/// <summary>
		///   List the ids of all entries.
		/// </summary>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationListResult{T,TOperationResult}" />
		///   that contains the <see cref="ListResult" />.
		/// </returns>
		Task<IOperationListResult<TEntryId, ListResult>> List();

		/// <summary>
		///   Read entries by its id.
		/// </summary>
		/// <param name="entryIds">The ids to be read.</param>
		/// <returns>A Task whose result contains the read results.</returns>
		Task<IEnumerable<IOperationResult<TEntry, TEntryId, ReadResult>>> Read(IEnumerable<TEntryId> entryIds);

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		Task<IOperationResult<TEntry, TEntryId, ReadResult>> Read(TEntryId entryId);

		/// <summary>
		///   Update an entry.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		Task<IOperationResult<TEntry, TEntryId, UpdateResult>> Update(TUpdateEntry entry);

		/// <summary>
		///   Update an entry.
		/// </summary>
		/// <param name="json">A json serialized <typeparamref name="TUpdateEntry" />.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		Task<IOperationResult<TEntry, TEntryId, UpdateResult>> Update(string json);
	}
}