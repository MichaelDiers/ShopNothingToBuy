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
		///   Delete a <see cref="StockItemDto" /> by <paramref name="id" />.
		/// </summary>
		/// <param name="id">The <see cref="StockItemDto.Id" /> to delete.</param>
		/// <returns>True if operation succeeds and false otherwise.</returns>
		Task<bool> Delete(Guid id);

		/// <summary>
		///   Read a <see cref="StockItem" /> by its id.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <returns>A <see cref="StockItemDto" /> if an item with given id exists, null otherwise.</returns>
		Task<StockItemDto> ReadById(Guid id);

		/// <summary>
		///   Update <see cref="StockItemDto.InStock" /> with given <paramref name="id" /> and increase
		///   <see cref="StockItemDto.InStock" /> by <paramref name="delta" />.
		/// </summary>
		/// <param name="id">The <see cref="StockItemDto.Id" />.</param>
		/// <param name="delta">Values that is added to <see cref="StockItemDto.InStock" />.</param>
		/// <returns>
		///   A <see cref="ValueTuple{T1, T2}" /> of <see cref="StockItemDto" /> and <see cref="bool" />.
		///   <see cref="StockItemDto" /> is null if not found by <paramref name="id" /> or a database error occurred.
		///   True indicates update was executed and otherwise false is returned.
		/// </returns>
		Task<(StockItemDto stockItem, bool isUpdated)> Update(Guid id, int delta);
	}
}