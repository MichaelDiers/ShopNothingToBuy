namespace OrdersApi.Server.Models
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///   Provides information of an order.
	/// </summary>
	public class Order : IOrder
	{
		/// <summary>
		///   The id of the customer that requested the order.
		/// </summary>
		public Guid CustomerId { get; set; }

		/// <summary>
		///   The unique id of the order.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		///   The positions that are part of the order.
		/// </summary>
		public IReadOnlyCollection<IPosition> Positions { get; set; }

		/// <summary>
		///   The current status of the order.
		/// </summary>
		public OrderStatus Status { get; set; }
	}
}