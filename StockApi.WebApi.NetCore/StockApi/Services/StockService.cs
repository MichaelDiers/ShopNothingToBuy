namespace StockApi.Services
{
	using System;
	using System.Threading.Tasks;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   Business logic for handling <see cref="StockItemDto" /> instances.
	/// </summary>
	public class StockService : IStockService
	{
		/// <summary>
		///   Database access for <see cref="StockItem" /> instances.
		/// </summary>
		private readonly IDatabaseService databaseService;

		/// <summary>
		///   Creates a new instance of <see cref="StockItem" />.
		/// </summary>
		/// <param name="databaseService">Database access for <see cref="StockItem" /> instances.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="databaseService" /> is null.</exception>
		public StockService(IDatabaseService databaseService)
		{
			this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
		}

		/// <summary>
		///   Clear all entries from database.
		/// </summary>
		/// <returns>True if operation succeeds and false otherwise.</returns>
		public async Task<bool> Clear()
		{
			return await this.databaseService.Clear();
		}

		/// <summary>
		///   Create a new <see cref="StockItemDto" /> in storage.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItemDto" /> to be created.</param>
		/// <returns>
		///   True if the <see cref="StockItemDto" /> is created or otherwise false.
		///   False indicates service errors or that the <see cref="StockItemDto.Id" /> already exists.
		/// </returns>
		public async Task<bool> Create(StockItemDto stockItem)
		{
			return await this.databaseService.Create(new StockItem(stockItem));
		}

		/// <summary>
		///   Delete a <see cref="StockItemDto" /> by <paramref name="id" />.
		/// </summary>
		/// <param name="id">The <see cref="StockItemDto.Id" /> to delete.</param>
		/// <returns>True if operation succeeds and false otherwise.</returns>
		public async Task<bool> Delete(Guid id)
		{
			return await this.databaseService.Delete(id);
		}

		/// <summary>
		///   Read a <see cref="StockItem" /> by its id.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <returns>A <see cref="StockItemDto" /> if an item with given id exists, null otherwise.</returns>
		public async Task<StockItemDto> ReadById(Guid id)
		{
			var stockItem = await this.databaseService.ReadById(id);
			return stockItem != null ? new StockItemDto(stockItem) : null;
		}

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
		public async Task<(StockItemDto stockItem, bool isUpdated)> Update(Guid id, int delta)
		{
			var stockItem = await this.databaseService.Update(id, delta);

			// unknown item
			if (stockItem is null)
			{
				return (null, false);
			}

			// no update operation
			if (delta == 0)
			{
				return (new StockItemDto(stockItem), true);
			}

			// enough items available to satisfy the requested amount of items
			// or the update increased the available items
			if (stockItem.InStock >= 0 || delta > 0)
			{
				return (new StockItemDto(stockItem), true);
			}

			// requested more than available
			// undo the operation
			stockItem = await this.databaseService.Update(id, -delta);
			return (new StockItemDto(stockItem), false);
		}
	}
}