namespace OrdersApi.Server.Models
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///   Describes the data of an order.
	/// </summary>
	public interface IOrder
	{
		/// <summary>
		///   The id of the customer that requested the order.
		/// </summary>
		Guid CustomerId { get; }

		/// <summary>
		///   The unique id of the order.
		/// </summary>
		Guid Id { get; }

		/// <summary>
		///   The positions that are part of the order.
		/// </summary>
		IReadOnlyCollection<IPosition> Positions { get; }

		/// <summary>
		///   The current status of the order.
		/// </summary>
		OrderStatus Status { get; }
	}
}