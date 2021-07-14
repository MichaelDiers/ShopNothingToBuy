namespace StockApi.Contracts
{
	using System;
	using System.Threading.Tasks;
	using StockApi.Models;

	/// <summary>
	///   Provides operations on <see cref="StockItemDto" /> instances.
	/// </summary>
	public interface IStockService
	{
		/// <summary>
		///   Clear all entries from database.
		/// </summary>
		/// <returns>True if operation succeeds and false otherwise.</returns>
		Task<bool> Clear();

		/// <summary>
		///   Create a new <see cref="StockItem" /> in storage.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItemDto" /> to be created.</param>
		/// <returns>
		///   True if the <see cref="StockItemDto" /> is created or otherwise false.
		///   False indicates service errors or that the <see cref="StockItemDto.Id" /> already exists.
		/// </returns>
		Task<bool> Create(StockItemDto stockItem);

		/// <summary>
		///   Read a <see cref="StockItem" /> by its id.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <returns>A <see cref="StockItemDto" /> if an item with given id exists, null otherwise.</returns>
		Task<StockItemDto> ReadById(Guid id);
	}
}