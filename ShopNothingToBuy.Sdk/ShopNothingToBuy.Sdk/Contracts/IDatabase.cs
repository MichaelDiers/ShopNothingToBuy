namespace ShopNothingToBuy.Sdk.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	///   Defines CRUD operations of a database.
	/// </summary>
	/// <typeparam name="TEntry">Type of a database entry.</typeparam>
	/// <typeparam name="TId">Type of the id of the database entry.</typeparam>
	public interface IDatabase<TEntry, in TId> where TEntry : IDatabaseEntry<TId>
	{
		/// <summary>
		///   Name of the database id field.
		/// </summary>
		const string DatabaseNameId = nameof(IDatabaseEntry<TId>.Id);

		/// <summary>
		///   Create a new entry in the database.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Created" /> if the entry is created. <see cref="DatabaseResult.AlreadyExists" /> if
		///   the entry already exists. <see cref="DatabaseResult.Error" /> if an error occurred.
		/// </returns>
		Task<DatabaseResult> Create(TEntry entry);

		/// <summary>
		///   Delete an entry with given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Deleted" /> if the entry with <paramref name="id" /> is deleted.
		///   <see cref="DatabaseResult.NotFound" /> if no entry exists with given id. <see cref="DatabaseResult.Error" /> if an
		///   error occurred.
		/// </returns>
		Task<DatabaseResult> Delete(TId id);

		/// <summary>
		///   Delete all entries from the database.
		/// </summary>
		/// <returns>
		///   <see cref="DatabaseResult.Deleted" /> if all entries are deleted. <see cref="DatabaseResult.Error" /> if an
		///   occurred.
		/// </returns>
		Task<DatabaseResult> Delete();

		/// <summary>
		///   Checks if an entry exists with <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>True if an entry exists and false otherwise.</returns>
		Task<bool> Exists(TId id);

		/// <summary>
		///   Read an entry with given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>An instance of <see cref="TEntry" /> if entry with <paramref name="id" /> exists and null otherwise.</returns>
		Task<TEntry> Read(TId id);

		/// <summary>
		///   Read all entries from the database.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="TEntry" />.</returns>
		Task<IEnumerable<TEntry>> Read();

		/// <summary>
		///   Update a database entry.
		/// </summary>
		/// <param name="entry">The entry to be updated.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Updated" /> if the operation succeeds. <see cref="DatabaseResult.NotFound" /> if no
		///   matching entry in database exists. <see cref="DatabaseResult.Error" /> if an error occurred.
		/// </returns>
		Task<DatabaseResult> Update(TEntry entry);
	}
}