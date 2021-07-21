namespace OrdersApi.Server.Interceptors
{
	using System;
	using System.Threading.Tasks;
	using Grpc.Core;
	using Grpc.Core.Interceptors;
	using Microsoft.Extensions.Configuration;

	/// <summary>
	///   Middleware validation for api keys.
	/// </summary>
	public class ApiKeyInterceptor : Interceptor
	{
		/// <summary>
		///   Name of the configuration entry containing the api key.
		/// </summary>
		private const string ConfigApiKeyName = "ApiKey";

		/// <summary>
		///   Name of the header containing the api key.
		/// </summary>
		private const string HeaderApiKeyName = "apikey";

		/// <summary>
		///   Access the <see cref="IConfiguration" /> of the application.
		/// </summary>
		private readonly IConfiguration configuration;

		/// <summary>
		///   Creates a new instance of <see cref="ApiKeyInterceptor" />.
		/// </summary>
		/// <param name="configuration">Access the <see cref="IConfiguration" /> of the application.</param>
		public ApiKeyInterceptor(IConfiguration configuration)
		{
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
		/// <exception cref="RpcException">Is thrown if the api key is invalid.</exception>
		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
			ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			var entry = context.RequestHeaders.Get(HeaderApiKeyName);
			if (entry is null || this.configuration[ConfigApiKeyName] != entry.Value)
			{
				throw new RpcException(new Status(StatusCode.Unauthenticated, "invalid api key"));
			}

			return await continuation(request, context);
		}
	}
}