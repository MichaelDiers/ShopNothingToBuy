namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;
	using OrdersApi.Server;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="Orders.OrdersClient.ClearAsync(Empty,CallOptions)" />.
	/// </summary>
	[Collection("Sequential")]
	public class ClearTests
	{
		[Fact]
		public async void ShouldFailIfApiKeyIsInvalid()
		{
			var request = new Empty();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ClearAsync(request, GRpcClient.CallOptions(Guid.NewGuid().ToString())));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsMissing()
		{
			var request = new Empty();
			await TestHelper.CheckStatusCode(async () => await GRpcClient.Client.ClearAsync(request));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsNotAGuid()
		{
			var request = new Empty();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ClearAsync(request, GRpcClient.CallOptions("notAGuid")));
		}

		[Fact]
		public async void ShouldSucceedForExistingOrder()
		{
			var createRequest = new CreateOrderRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			createRequest.Positions.Add(
				new PositionDto
				{
					Amount = 10,
					ProductId = Guid.NewGuid().ToString()
				});
			var createResponse = await GRpcClient.Client.CreateOrderAsync(
				createRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Equal(OrderStatusDto.Created, createResponse.OrderStatus);

			var clearRequest = new Empty();
			var _ = await GRpcClient.Client.ClearAsync(
				clearRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));

			var readRequest = new OrderIdRequest
			{
				CustomerId = createRequest.CustomerId,
				OrderId = createResponse.OrderId
			};
			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.ReadOrderAsync(readRequest, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
		}
	}
}