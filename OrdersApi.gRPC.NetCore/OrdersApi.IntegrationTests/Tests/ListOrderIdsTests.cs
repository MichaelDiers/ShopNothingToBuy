namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using Google.Protobuf.WellKnownTypes;
	using OrdersApi.Server;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="Orders.OrdersClient.ListOrderIdsAsync(Empty,Grpc.Core.CallOptions)" />.
	/// </summary>
	[Collection("Sequential")]
	public class ListOrderIdsTests
	{
		[Fact]
		public async void ShouldFailIfApiKeyIsInvalid()
		{
			var request = new Empty();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ListOrderIdsAsync(request, GRpcClient.CallOptions(Guid.NewGuid().ToString())));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsMissing()
		{
			var request = new Empty();
			await TestHelper.CheckStatusCode(async () => await GRpcClient.Client.ListOrderIdsAsync(request));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsNotAGuid()
		{
			var request = new Empty();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ListOrderIdsAsync(request, GRpcClient.CallOptions("notAGuid")));
		}

		[Fact]
		public async void ShouldSucceed()
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

			var listOrderIdsRequest = new Empty();
			var listOrderIdsResponse = await GRpcClient.Client.ListOrderIdsAsync(
				listOrderIdsRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Contains(createResponse.OrderId, listOrderIdsResponse.OrderIds);
		}
	}
}