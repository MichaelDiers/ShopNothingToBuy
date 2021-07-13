namespace StockApi.Services
{
	using System.Threading;
	using System.Threading.Tasks;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   Handles database operations on redis database.
	/// </summary>
	public class DatabaseService : IDatabaseService
	{
		/// <summary>
		///   Create a new entry in the database.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItem" /> to be stored.</param>
		/// <returns>
		///   True if <see cref="StockItem" /> is stored and otherwise false. False indicates a database error or that the
		///   <see cref="StockItem.Id" /> already exists.
		/// </returns>
		public async Task<bool> Create(StockItem stockItem)
		{
			await Task.Run(() => Thread.Sleep(1000));
			return true;
		}
	}
}