namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using Grpc.Core;
	using OrdersApi.Server;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="Orders.OrdersClient.ListOrderIdsByCustomerAsync(CustomerIdRequest,CallOptions)" />.
	/// </summary>
	[Collection("Sequential")]
	public class ListOrderIdsByCustomerTests
	{
		[Fact]
		public async void ShouldFailIfApiKeyIsInvalid()
		{
			var request = new CustomerIdRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ListOrderIdsByCustomerAsync(
						request,
						GRpcClient.CallOptions(Guid.NewGuid().ToString())));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsMissing()
		{
			var request = new CustomerIdRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			await TestHelper.CheckStatusCode(async () => await GRpcClient.Client.ListOrderIdsByCustomerAsync(request));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsNotAGuid()
		{
			var request = new CustomerIdRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ListOrderIdsByCustomerAsync(request, GRpcClient.CallOptions("notAGuid")));
		}

		[Fact]
		public async void ShouldFailIfCustomerIdIsNotAGuid()
		{
			var request = new CustomerIdRequest
			{
				CustomerId = "notAGuid"
			};

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ListOrderIdsByCustomerAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(CustomerIdRequest.CustomerId));
		}

		[Fact]
		public async void ShouldFailIfCustomerIdIsNotSet()
		{
			var request = new CustomerIdRequest();

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ListOrderIdsByCustomerAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(CustomerIdRequest.CustomerId));
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

			var listOrderIdsRequest = new CustomerIdRequest
			{
				CustomerId = createRequest.CustomerId
			};
			var listOrderIdsResponse = await GRpcClient.Client.ListOrderIdsByCustomerAsync(
				listOrderIdsRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Single(listOrderIdsResponse.OrderIds);
			Assert.Contains(createResponse.OrderId, listOrderIdsResponse.OrderIds);
		}
	}
}