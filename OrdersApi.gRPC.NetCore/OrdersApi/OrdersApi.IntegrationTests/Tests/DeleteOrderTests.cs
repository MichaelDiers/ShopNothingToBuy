namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using Grpc.Core;
	using OrdersApi.Server;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="Orders.OrdersClient.DeleteOrderAsync(OrderIdRequest,CallOptions)" />.
	/// </summary>
	[Collection("Sequential")]
	public class DeleteOrderTests
	{
		[Fact]
		public async void ShouldFailForExistingCustomerIdButNotMatchingOrderId()
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

			var deleteRequest = new OrderIdRequest
			{
				CustomerId = createRequest.CustomerId,
				OrderId = Guid.NewGuid().ToString()
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.DeleteOrderAsync(
					deleteRequest,
					GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
		}

		[Fact]
		public async void ShouldFailForExistingOrderIdButNotMatchingCustomerId()
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

			var deleteRequest = new OrderIdRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = createResponse.OrderId
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.DeleteOrderAsync(
					deleteRequest,
					GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsInvalid()
		{
			var request = new OrderIdRequest();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.DeleteOrderAsync(request, GRpcClient.CallOptions(Guid.NewGuid().ToString())));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsMissing()
		{
			var request = new OrderIdRequest();
			await TestHelper.CheckStatusCode(async () => await GRpcClient.Client.DeleteOrderAsync(request));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsNotAGuid()
		{
			var request = new OrderIdRequest();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.DeleteOrderAsync(request, GRpcClient.CallOptions("notAGuid")));
		}

		[Fact]
		public async void ShouldFailIfCustomerIdIsNotAGuid()
		{
			var request = new OrderIdRequest
			{
				CustomerId = "noGuid",
				OrderId = Guid.NewGuid().ToString()
			};
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.DeleteOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(OrderIdRequest.CustomerId));
		}

		[Fact]
		public async void ShouldFailIfCustomerIdIsNotSet()
		{
			var request = new OrderIdRequest
			{
				OrderId = Guid.NewGuid().ToString()
			};
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.DeleteOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(OrderIdRequest.CustomerId));
		}

		[Fact]
		public async void ShouldFailIfOrderIdIsNotAGuid()
		{
			var request = new OrderIdRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = "noGuid"
			};
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.DeleteOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(OrderIdRequest.OrderId));
		}

		[Fact]
		public async void ShouldFailIfOrderIdIsNotSet()
		{
			var request = new OrderIdRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.DeleteOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(OrderIdRequest.OrderId));
		}

		[Fact]
		public async void ShouldFailIfOrderIdIsUnknown()
		{
			var request = new OrderIdRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = Guid.NewGuid().ToString()
			};
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.DeleteOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
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

			var deleteRequest = new OrderIdRequest
			{
				CustomerId = createRequest.CustomerId,
				OrderId = createResponse.OrderId
			};
			var deleteResponse = await GRpcClient.Client.DeleteOrderAsync(
				deleteRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.NotNull(deleteResponse);
		}
	}
}