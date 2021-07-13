namespace StockApi.Contracts
{
	using System.Threading.Tasks;
	using StockApi.Models;

	/// <summary>
	///   Provides operations for database access.
	/// </summary>
	public interface IDatabaseService
	{
		/// <summary>
		///   Create a new <see cref="StockItem" />.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItem" /> to be created.</param>
		/// <returns>
		///   True if the item is created and false otherwise. False indicates a
		///   database error or that the an item with <see cref="StockItem.Id" /> already exists.
		/// </returns>
		Task<bool> Create(StockItem stockItem);
	}
}