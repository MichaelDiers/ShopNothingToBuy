namespace OrdersApi.Server.Interceptors
{
	using System;
	using System.Threading.Tasks;
	using Grpc.Core;
	using Grpc.Core.Interceptors;

	/// <summary>
	///   Middleware for validating <see cref="OrderIdRequest" /> instances.
	/// </summary>
	public class OrderIdRequestInterceptor : Interceptor
	{
		/// <summary>
		///   Name of the customer id entry in <see cref="ServerCallContext.UserState" />.
		/// </summary>
		public const string CustomerIdName = "customerId";

		/// <summary>
		///   Name of the order id entry in <see cref="ServerCallContext.UserState" />.
		/// </summary>
		public const string OrderIdName = "orderId";

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
		/// <exception cref="RpcException">Thrown if <see cref="OrderIdRequest" /> is not valid.</exception>
		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
			TRequest request,
			ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			if (request is OrderIdRequest orderIdRequest)
			{
				ValidateAndSetToContext(orderIdRequest, context);
			}

			return await continuation(request, context);
		}

		/// <summary>
		///   Validate the <see cref="OrderIdRequest.OrderId" /> of the request.
		/// </summary>
		/// <param name="request">The request containing the order id.</param>
		/// <param name="context">If the order id is valid, it will be added to <see cref="ServerCallContext.UserState" />.</param>
		/// <exception cref="RpcException">Thrown if the order id not valid.</exception>
		private static void ValidateAndSetToContext(OrderIdRequest request, ServerCallContext context)
		{
			if (!ValidateAndSetToContext(context, request.OrderId, OrderIdName))
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request.OrderId)));
			}

			if (!ValidateAndSetToContext(context, request.CustomerId, CustomerIdName))
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request.CustomerId)));
			}
		}

		/// <summary>
		///   Check if the given guid is valid add it to the <see cref="ServerCallContext.UserState" />.
		/// </summary>
		/// <param name="context">
		///   A valid guid is added to <see cref="ServerCallContext.UserState" /> with key
		///   <paramref name="userStateName" />.
		/// </param>
		/// <param name="guid">The guid that is validated.</param>
		/// <param name="userStateName">The key used in <see cref="ServerCallContext.UserState" /> for the valid guid.</param>
		/// <returns>True if the guid is valid and false otherwise.</returns>
		private static bool ValidateAndSetToContext(ServerCallContext context, string guid, string userStateName)
		{
			if (string.IsNullOrWhiteSpace(guid) || !Guid.TryParse(guid, out var parsedGuid) || parsedGuid == Guid.Empty)
			{
				return false;
			}

			context.UserState.Add(userStateName, parsedGuid);
			return true;
		}
	}
}