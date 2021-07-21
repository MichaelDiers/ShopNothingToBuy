namespace OrdersApi.Server.Interceptors
{
	using System.Threading.Tasks;
	using Grpc.Core;
	using OrdersApi.Server.Extensions;

	/// <summary>
	///   Middleware for validating <see cref="UpdateStatusRequest" /> instances.
	/// </summary>
	public class UpdateStatusRequestInterceptor : BaseInterceptor
	{
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
		/// <exception cref="RpcException">Thrown if <see cref="UpdateStatusRequest" /> is not valid.</exception>
		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
			TRequest request,
			ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			if (request is UpdateStatusRequest updateStatusRequest)
			{
				ValidateGuidAndSetToContext(context, updateStatusRequest.OrderId, nameof(updateStatusRequest.OrderId));
				ValidateGuidAndSetToContext(context, updateStatusRequest.CustomerId, nameof(updateStatusRequest.CustomerId));
				var newStatus = updateStatusRequest.NewStatus.ToOrderStatus();
				context.UserState.Add(nameof(updateStatusRequest.NewStatus), newStatus);
			}

			return await continuation(request, context);
		}
	}
}