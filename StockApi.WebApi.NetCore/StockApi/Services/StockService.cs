namespace StockApi.Services
{
	using System;
	using System.Threading.Tasks;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   Business logic for handling <see cref="StockItem" /> instances.
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
		///   Create a new <see cref="StockItem" /> in storage.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItem" /> to be created.</param>
		/// <returns>
		///   True if the <see cref="StockItem" /> is created or otherwise false.
		///   False indicates service errors or that the <see cref="StockItem.Id" /> already exists.
		/// </returns>
		public async Task<bool> Create(StockItem stockItem)
		{
			return await this.databaseService.Create(stockItem);
		}
	}
}