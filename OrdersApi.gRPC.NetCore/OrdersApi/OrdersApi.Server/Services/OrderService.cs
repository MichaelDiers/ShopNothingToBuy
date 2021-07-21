namespace OrdersApi.Server.Services
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using OrdersApi.Server.Contracts;
	using OrdersApi.Server.Models;

	/// <summary>
	///   Provides operations for processing orders.
	/// </summary>
	public class OrderService : IOrderService
	{
		/// <summary>
		///   Provides database operations of orders.
		/// </summary>
		private readonly IDatabaseService databaseService;

		/// <summary>
		///   Create a new instance of <see cref="OrderService" />.
		/// </summary>
		/// <param name="databaseService">A service providing database operations.</param>
		public OrderService(IDatabaseService databaseService)
		{
			this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
		}

		/// <summary>
		///   Delete all orders from storage.
		/// </summary>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task Clear()
		{
			await this.databaseService.Clear();
		}

		/// <summary>
		///   Create a new order in the database.
		/// </summary>
		/// <param name="order">The order to be created.</param>
		/// <returns>The created and enriched <see cref="IOrder" />.</returns>
		public async Task<IOrder> CreateOrder(Order order)
		{
			order.Id = Guid.NewGuid();
			order.Status = OrderStatus.Created;
			await this.databaseService.Create(order);
			return order;
		}

		/// <summary>
		///   Delete an order of a customer by its id.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		public async Task<bool> DeleteOrder(Guid customerId, Guid orderId)
		{
			return await this.databaseService.DeleteOrder(customerId, orderId);
		}

		/// <summary>
		///   List all known ids of orders.
		/// </summary>
		/// <returns>A list of order ids.</returns>
		public async Task<IEnumerable<Guid>> ListOrderIds()
		{
			return await this.databaseService.ListOrderIds();
		}

		/// <summary>
		///   List all known ids of orders for given customer id.
		/// </summary>
		/// <param name="customerId">The id of the customer id.</param>
		/// <returns>A list of order ids.</returns>
		public async Task<IEnumerable<Guid>> ListOrderIds(Guid customerId)
		{
			return await this.databaseService.ListOrderIds(customerId);
		}

		/// <summary>
		///   Read an order by its id for a specified customer.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <returns>
		///   An instance of <see cref="IOrder" /> or null if no matching order is found.
		/// </returns>
		public async Task<IOrder> ReadOrder(Guid customerId, Guid orderId)
		{
			return await this.databaseService.ReadOrder(customerId, orderId);
		}

		/// <summary>
		///   Update the order status for an order of a given customer to a new status.
		/// </summary>
		/// <param name="customerId">The customer that owns the order.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <param name="newOrderStatus">The new status of the order.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		public async Task<bool> UpdateOrderStatus(Guid customerId, Guid orderId, OrderStatus newOrderStatus)
		{
			return await this.databaseService.UpdateOrderStatus(customerId, orderId, newOrderStatus);
		}
	}
}