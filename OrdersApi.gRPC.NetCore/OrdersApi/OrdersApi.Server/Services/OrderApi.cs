namespace OrdersApi.Server.Services
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;
	using OrdersApi.Server.Contracts;
	using OrdersApi.Server.Extensions;
	using OrdersApi.Server.Interceptors;
	using OrdersApi.Server.Models;

	/// <summary>
	///   Implementation of the <see cref="OrderApi" />.
	/// </summary>
	public class OrderApi : Orders.OrdersBase
	{
		/// <summary>
		///   The business logic for handling orders.
		/// </summary>
		private readonly IOrderService orderService;

		/// <summary>
		///   Create a new instance of <see cref="OrderApi" />.
		/// </summary>
		/// <param name="orderService">Business logic for handling orders.</param>
		public OrderApi(IOrderService orderService)
		{
			this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
		}

		/// <summary>
		///   Delete all entries from the database.
		/// </summary>
		/// <param name="request">A request that contains no data.</param>
		/// <param name="context">The <see cref="ServerCallContext" />.</param>
		/// <returns>An empty response.</returns>
		public override async Task<Empty> Clear(Empty request, ServerCallContext context)
		{
			await this.orderService.Clear();
			return new Empty();
		}

		/// <summary>
		///   Create a new order in the database.
		/// </summary>
		/// <param name="request">The request containing order information.</param>
		/// <param name="context">The <see cref="ServerCallContext" />.</param>
		/// <returns>
		///   An <see cref="OrderResponse" /> containing the original request data and a new
		///   <see cref="OrderResponse.OrderId" /> and <see cref="OrderResponse.OrderStatus" />.
		/// </returns>
		public override async Task<OrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
		{
			var order = await this.orderService.CreateOrder(context.Get<Order>(CreateOrderRequestInterceptor.OrderName));

			var response = new OrderResponse
			{
				OrderId = order.Id.ToString(),
				OrderStatus = order.Status.ToOrderStatusDto()
			};

			response.Positions.AddRange(
				order.Positions.Select(
					position => new PositionDto
					{
						Amount = position.Amount,
						ProductId = position.ProductId.ToString()
					}));
			return response;
		}

		/// <summary>
		///   Delete an order from the database.
		/// </summary>
		/// <param name="request">The request containing the order id.</param>
		/// <param name="context">The <see cref="ServerCallContext" />.</param>
		/// <returns>An empty request.</returns>
		public override async Task<Empty> DeleteOrder(OrderIdRequest request, ServerCallContext context)
		{
			var customerId = context.Get<Guid>(OrderIdRequestInterceptor.CustomerIdName);
			var orderId = context.Get<Guid>(OrderIdRequestInterceptor.OrderIdName);

			if (!await this.orderService.DeleteOrder(customerId, orderId))
			{
				context.Status = new Status(StatusCode.NotFound, string.Empty);
			}

			return new Empty();
		}

		/// <summary>
		///   Create a list of all known order ids.
		/// </summary>
		/// <param name="request">An empty request.</param>
		/// <param name="context">The <see cref="ServerCallContext" />.</param>
		/// <returns>The response contains a list of all order ids.</returns>
		public override async Task<ListOrderIdsResponse> ListOrderIds(Empty request, ServerCallContext context)
		{
			var ids = await this.orderService.ListOrderIds();
			var response = new ListOrderIdsResponse();
			response.OrderIds.AddRange(ids.Select(id => id.ToString()));
			return response;
		}

		/// <summary>
		///   Create a list of all known order ids for a given customer.
		/// </summary>
		/// <param name="request">An empty request.</param>
		/// <param name="context">The <see cref="ServerCallContext" />.</param>
		/// <returns>The response contains a list of all order ids.</returns>
		public override async Task<ListOrderIdsResponse> ListOrderIdsByCustomer(
			CustomerIdRequest request,
			ServerCallContext context)
		{
			var customerId = context.Get<Guid>(CustomerIdRequestInterceptor.CustomerIdName);
			var ids = await this.orderService.ListOrderIds(customerId);
			var response = new ListOrderIdsResponse();
			response.OrderIds.AddRange(ids.Select(id => id.ToString()));
			return response;
		}

		/// <summary>
		///   Read the data of an order.
		/// </summary>
		/// <param name="request">The request contains the order id of the order to read.</param>
		/// <param name="context">The <see cref="ServerCallContext" />.</param>
		/// <returns>The data of the order.</returns>
		public override async Task<OrderResponse> ReadOrder(OrderIdRequest request, ServerCallContext context)
		{
			var customerId = context.Get<Guid>(OrderIdRequestInterceptor.CustomerIdName);
			var orderId = context.Get<Guid>(OrderIdRequestInterceptor.OrderIdName);
			var order = await this.orderService.ReadOrder(customerId, orderId);
			if (order is null)
			{
				throw new RpcException(new Status(StatusCode.NotFound, string.Empty));
			}

			var response = new OrderResponse
			{
				CustomerId = order.CustomerId.ToString(),
				OrderId = order.Id.ToString(),
				OrderStatus = order.Status.ToOrderStatusDto()
			};

			response.Positions.AddRange(
				order.Positions.Select(
					position => new PositionDto
					{
						Amount = position.Amount,
						ProductId = position.ProductId.ToString()
					}));

			return response;
		}

		/// <summary>
		///   Updates the <see cref="Order.Status" /> in the database.
		/// </summary>
		/// <param name="request">The <see cref="UpdateStatusRequest" />.</param>
		/// <param name="context">The <see cref="ServerCallContext" />.</param>
		/// <returns>An <see cref="Empty" /> response.</returns>
		public override async Task<Empty> UpdateStatus(UpdateStatusRequest request, ServerCallContext context)
		{
			var customerId = context.Get<Guid>(nameof(request.CustomerId));
			var orderId = context.Get<Guid>(nameof(request.OrderId));
			var newStatus = context.Get<OrderStatus>(nameof(request.NewStatus));

			if (!await this.orderService.UpdateOrderStatus(customerId, orderId, newStatus))
			{
				context.Status = new Status(StatusCode.NotFound, string.Empty);
			}

			return new Empty();
		}
	}
}