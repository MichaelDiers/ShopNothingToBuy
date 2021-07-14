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
		///   Read a <see cref="StockItem" /> by its id.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <returns>A <see cref="StockItemDto" /> if an item with given id exists, null otherwise.</returns>
		public async Task<StockItemDto> ReadById(Guid id)
		{
			var stockItem = await this.databaseService.ReadById(id);
			return stockItem != null ? new StockItemDto(stockItem) : null;
		}
	}
}