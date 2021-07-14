namespace StockApi.Contracts
{
	using System;
	using System.Threading.Tasks;
	using StockApi.Models;

	/// <summary>
	///   Provides operations for database access.
	/// </summary>
	public interface IDatabaseService
	{
		/// <summary>
		///   Clear all entries from database.
		/// </summary>
		/// <returns>True if operation succeeds and false otherwise.</returns>
		Task<bool> Clear();

		/// <summary>
		///   Create a new <see cref="StockItem" />.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItem" /> to be created.</param>
		/// <returns>
		///   True if the item is created and false otherwise. False indicates a
		///   database error or that the an item with <see cref="StockItem.Id" /> already exists.
		/// </returns>
		Task<bool> Create(StockItem stockItem);

		/// <summary>
		///   Read a <see cref="StockItem" /> by its id.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <returns>A <see cref="StockItem" /> if an item with given id exists, null otherwise.</returns>
		Task<StockItem> ReadById(Guid id);
	}
}