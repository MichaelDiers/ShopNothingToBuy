namespace StockApi.Contracts
{
	using System.Threading.Tasks;
	using StockApi.Models;

	/// <summary>
	///   Provides operations on <see cref="StockItem" /> instances.
	/// </summary>
	public interface IStockService
	{
		/// <summary>
		///   Create a new <see cref="StockItem" /> in storage.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItem" /> to be created.</param>
		/// <returns>
		///   True if the <see cref="StockItem" /> is created or otherwise false.
		///   False indicates service errors or that the <see cref="StockItem.Id" /> already exists.
		/// </returns>
		Task<bool> Create(StockItem stockItem);
	}
}