namespace StockApi.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using StockApi.Validation;

	/// <summary>
	///   Defines a <see cref="StockItem" />.
	/// </summary>
	public class StockItem
	{
		/// <summary>
		///   Creates a new instance of <see cref="StockItem" />.
		/// </summary>
		public StockItem()
		{
		}

		/// <summary>
		///   Create a new instance of <see cref="StockItem" />.
		/// </summary>
		/// <param name="stockItemDto">Specifies the values of the new instance.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="stockItemDto" /> or one of its properties is null.</exception>
		public StockItem(StockItemDto stockItemDto)
		{
			if (stockItemDto?.Id is null || stockItemDto.InStock is null)
			{
				throw new ArgumentNullException(nameof(stockItemDto), "Invalid object!");
			}

			this.Id = stockItemDto.Id.Value;
			this.InStock = stockItemDto.InStock.Value;
		}

		/// <summary>
		///   The id of a <see cref="StockItem" />. <see cref="Guid.Empty" /> is treated as an invalid id.
		/// </summary>
		[Required]
		[NonEmptyGuid]
		public Guid Id { get; set; }

		/// <summary>
		///   The amount of this <see cref="StockItem" /> in stock.
		/// </summary>
		[Required]
		[Range(0, 10000)]
		public int InStock { get; set; }
	}
}