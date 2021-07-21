namespace OrdersApi.Server.Models
{
	using System;

	/// <summary>
	///   Describes a position of an order.
	/// </summary>
	public class Position : IPosition
	{
		/// <summary>
		///   The amount of the product defined as <see cref="ProductId" />.
		/// </summary>
		public uint Amount { get; set; }

		/// <summary>
		///   The id of the product.
		/// </summary>
		public Guid ProductId { get; set; }
	}
}