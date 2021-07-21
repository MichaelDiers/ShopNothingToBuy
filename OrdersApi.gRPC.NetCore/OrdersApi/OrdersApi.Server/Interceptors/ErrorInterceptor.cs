namespace OrdersApi.Server.Interceptors
{
	using System;
	using System.Threading.Tasks;
	using Grpc.Core;
	using Grpc.Core.Interceptors;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///   A global error handler for the application.
	/// </summary>
	public class ErrorInterceptor : Interceptor
	{
		/// <summary>
		///   Used for logging errors.
		/// </summary>
		private readonly ILogger<ErrorInterceptor> logger;

		/// <summary>
		///   Create a new instance of <see cref="ErrorInterceptor" />.
		/// </summary>
		/// <param name="logger">Used for logging errors.</param>
		public ErrorInterceptor(ILogger<ErrorInterceptor> logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

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
		/// <exception cref="RpcException">If an <see cref="Exception" /> is catched and rethrown as <see cref="RpcException" />.</exception>
		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
			ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			try
			{
				return await continuation(request, context);
			}
			catch (RpcException)
			{
				throw;
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, ex.Message);
				throw new RpcException(new Status(StatusCode.Internal, "global error handling"));
			}
		}
	}
}