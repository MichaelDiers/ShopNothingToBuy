namespace StockApi.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using StockApi.Validation;

	/// <summary>
	///   Data transfer object for <see cref="StockItem" />.
	/// </summary>
	public class StockItemDto
	{
		/// <summary>
		///   Creates a new instance of <see cref="StockItemDto" />.
		/// </summary>
		public StockItemDto()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="StockItemDto" />.
		/// </summary>
		/// <param name="stockItem">Specifies the values of the new instance.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="stockItem" /> is null.</exception>
		public StockItemDto(StockItem stockItem)
		{
			if (stockItem == null)
			{
				throw new ArgumentNullException(nameof(stockItem));
			}

			this.Id = stockItem.Id;
			this.InStock = stockItem.InStock;
		}

		/// <summary>
		///   The id of a <see cref="StockItem" />. <see cref="Guid.Empty" /> is treated as an invalid id.
		/// </summary>
		[Required]
		[NonEmptyGuid]
		public Guid? Id { get; set; }

		/// <summary>
		///   The amount of this <see cref="StockItem" /> in stock.
		/// </summary>
		[Required]
		[Range(0, 10000)]
		public int? InStock { get; set; }
	}
}