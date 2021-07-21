namespace OrdersApi.Server.Interceptors
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Grpc.Core;
	using Grpc.Core.Interceptors;
	using OrdersApi.Server.Models;

	/// <summary>
	///   Middleware for validating <see cref="CreateOrderRequest" /> instances.
	/// </summary>
	public class CreateOrderRequestInterceptor : Interceptor
	{
		/// <summary>
		///   Name of the <see cref="ServerCallContext.UserState" /> entry for the order.
		/// </summary>
		public const string OrderName = nameof(OrderName);

		/// <summary>
		///   Server-side handler for intercepting and incoming unary call.
		/// </summary>
		/// <typeparam name="TRequest">Request message type for this method.</typeparam>
		/// <typeparam name="TResponse">Response message type for this method.</typeparam>
		/// <param name="request">The request value of the incoming invocation.</param>
		/// <param name="context">
		///   An instance of <see cref="Grpc.Core.ServerCallContext" /> representing
		///   the context of the invocation.
		/// </param>
		/// <param name="continuation">
		///   A delegate that asynchronously proceeds with the invocation, calling
		///   the next interceptor in the chain, or the service request handler,
		///   in case of the last interceptor and return the response value of
		///   the RPC. The interceptor can choose to call it zero or more times
		///   at its discretion.
		/// </param>
		/// <returns>
		///   A future representing the response value of the RPC. The interceptor
		///   can simply return the return value from the continuation intact,
		///   or an arbitrary response value as it sees fit.
		/// </returns>
		/// <exception cref="RpcException">Is thrown if <see cref="CreateOrderRequest" /> is not valid.</exception>
		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
			TRequest request,
			ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			if (request is CreateOrderRequest createOrderRequest)
			{
				var order = ValidateAndCreate(createOrderRequest);
				if (order is null)
				{
					throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(createOrderRequest.Positions)));
				}

				context.UserState[OrderName] = order;
			}

			return await continuation(request, context);
		}

		/// <summary>
		///   Validate <paramref name="request" /> and create a new <see cref="Order" /> instance.
		/// </summary>
		/// <param name="request">The request that is validated.</param>
		/// <returns>An instance of <see cref="Order" /> created from the <paramref name="request" /> value.</returns>
		private static Order ValidateAndCreate(CreateOrderRequest request)
		{
			if (request is null || !Guid.TryParse(request.CustomerId, out var customerId) || customerId == Guid.Empty)
			{
				return null;
			}

			var positions = ValidateAndCreate(request.Positions);
			if (positions == null)
			{
				return null;
			}

			return new Order
			{
				CustomerId = customerId,
				Positions = positions
			};
		}

		/// <summary>
		///   Validate the <paramref name="positionDtos" /> and create a list of <see cref="Position" /> instances.
		/// </summary>
		/// <param name="positionDtos">The <see cref="PositionDto" /> instances to be validated.</param>
		/// <returns>A list of <see cref="Position" /> instances created from <paramref name="positionDtos" />.</returns>
		private static IReadOnlyCollection<Position> ValidateAndCreate(IReadOnlyCollection<PositionDto> positionDtos)
		{
			if (positionDtos is null || positionDtos.Count == 0)
			{
				return null;
			}

			var positions = new List<Position>();
			foreach (var positionDto in positionDtos)
			{
				if (positionDto != null
				    && positionDto.Amount > 0
				    && !string.IsNullOrWhiteSpace(positionDto.ProductId)
				    && Guid.TryParse(positionDto.ProductId, out var productId)
				    && productId != Guid.Empty)
				{
					positions.Add(
						new Position
						{
							Amount = positionDto.Amount,
							ProductId = productId
						});
				}
				else
				{
					return null;
				}
			}

			return positions;
		}
	}
}