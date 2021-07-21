namespace OrdersApi.IntegrationTests.Tests
{
	using Grpc.Core;
	using Grpc.Net.Client;
	using OrdersApi.Server;

	/// <summary>
	///   Provides <see cref="Orders.OrdersClient" /> operations.
	/// </summary>
	public static class GRpcClient
	{
		/// <summary>
		///   Initializes the <see cref="Client" />.
		/// </summary>
		static GRpcClient()
		{
			var channel = GrpcChannel.ForAddress("https://localhost:5001");
			Client = new Orders.OrdersClient(channel);
		}

		/// <summary>
		///   A <see cref="Orders.OrdersClient" /> used for calling the matching service.
		/// </summary>
		public static Orders.OrdersClient Client { get; }

		/// <summary>
		///   Adds an <paramref name="apiKey" /> to the <see cref="CallOptions" /> and returns them.
		/// </summary>
		/// <param name="apiKey">The api key that is send to the server.</param>
		/// <returns>The <see cref="CallOptions" /> used for a server call.</returns>
		public static CallOptions CallOptions(string apiKey)
		{
			if (string.IsNullOrWhiteSpace(apiKey))
			{
				return new CallOptions();
			}

			return new CallOptions(
				new Metadata
				{
					{"ApiKey", apiKey}
				});
		}
	}
}