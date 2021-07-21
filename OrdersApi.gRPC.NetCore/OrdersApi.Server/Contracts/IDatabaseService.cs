namespace OrdersApi.Server.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using OrdersApi.Server.Models;

	/// <summary>
	///   Provides database operations.
	/// </summary>
	public interface IDatabaseService
	{
		/// <summary>
		///   Delete the complete collection of storage entries.
		/// </summary>
		/// <returns>A <see cref="Task" />.</returns>
		Task Clear();

		/// <summary>
		///   Create a new order in the database.
		/// </summary>
		/// <param name="order">The order to be created.</param>
		/// <returns>A <see cref="Task" />.</returns>
		Task Create(Order order);

		/// <summary>
		///   Delete an order of a customer by its id.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		Task<bool> DeleteOrder(Guid customerId, Guid orderId);

		/// <summary>
		///   List all known ids of orders.
		/// </summary>
		/// <returns>A list of order ids.</returns>
		Task<IEnumerable<Guid>> ListOrderIds();

		/// <summary>
		///   List all known ids of orders for given customer id.
		/// </summary>
		/// <param name="customerId">The id of the customer id.</param>
		/// <returns>A list of order ids.</returns>
		Task<IEnumerable<Guid>> ListOrderIds(Guid customerId);

		/// <summary>
		///   Read an order by its id for a specified customer.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <returns>
		///   An instance of <see cref="Order" /> or null if no matching order is found.
		/// </returns>
		Task<Order> ReadOrder(Guid customerId, Guid orderId);

		/// <summary>
		///   Update the order status for an order of a given customer to a new status.
		/// </summary>
		/// <param name="customerId">The customer that owns the order.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <param name="newOrderStatus">The new status of the order.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		Task<bool> UpdateOrderStatus(Guid customerId, Guid orderId, OrderStatus newOrderStatus);
	}
}