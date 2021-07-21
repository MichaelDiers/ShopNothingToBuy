namespace OrdersApi.Server.Models
{
	using System;

	/// <summary>
	///   Describes a position of an order.
	/// </summary>
	public interface IPosition
	{
		/// <summary>
		///   The amount of the product defined as <see cref="ProductId" />.
		/// </summary>
		uint Amount { get; }

		/// <summary>
		///   The id of the product.
		/// </summary>
		Guid ProductId { get; }
	}
}