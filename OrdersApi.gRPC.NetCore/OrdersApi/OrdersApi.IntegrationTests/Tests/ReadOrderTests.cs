namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using System.Linq;
	using Grpc.Core;
	using OrdersApi.Server;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="Orders.OrdersClient.ReadOrderAsync(OrderIdRequest,CallOptions)" />.
	/// </summary>
	[Collection("Sequential")]
	public class ReadOrderTests
	{
		[Fact]
		public async void ShouldFailForExistingCustomerIdAndNotMatchingOrderId()
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
			createRequest.Positions.Add(
				new PositionDto
				{
					Amount = 22,
					ProductId = Guid.NewGuid().ToString()
				});
			createRequest.Positions.Add(
				new PositionDto
				{
					Amount = 33,
					ProductId = Guid.NewGuid().ToString()
				});
			var createResponse = await GRpcClient.Client.CreateOrderAsync(
				createRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Equal(OrderStatusDto.Created, createResponse.OrderStatus);

			var readRequest = new OrderIdRequest
			{
				CustomerId = createRequest.CustomerId,
				OrderId = Guid.NewGuid().ToString()
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.ReadOrderAsync(
					readRequest,
					GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
		}

		[Fact]
		public async void ShouldFailForExistingOrderIdAndNotMatchingCustomerId()
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
			createRequest.Positions.Add(
				new PositionDto
				{
					Amount = 22,
					ProductId = Guid.NewGuid().ToString()
				});
			createRequest.Positions.Add(
				new PositionDto
				{
					Amount = 33,
					ProductId = Guid.NewGuid().ToString()
				});
			var createResponse = await GRpcClient.Client.CreateOrderAsync(
				createRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Equal(OrderStatusDto.Created, createResponse.OrderStatus);

			var readRequest = new OrderIdRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = createResponse.OrderId
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.ReadOrderAsync(
					readRequest,
					GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsInvalid()
		{
			var request = new OrderIdRequest();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ReadOrderAsync(request, GRpcClient.CallOptions(Guid.NewGuid().ToString())));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsMissing()
		{
			var request = new OrderIdRequest();
			await TestHelper.CheckStatusCode(async () => await GRpcClient.Client.ReadOrderAsync(request));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsNotAGuid()
		{
			var request = new OrderIdRequest();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.ReadOrderAsync(request, GRpcClient.CallOptions("notAGuid")));
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
					await GRpcClient.Client.ReadOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
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
					await GRpcClient.Client.ReadOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
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
					await GRpcClient.Client.ReadOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
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
					await GRpcClient.Client.ReadOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
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
					await GRpcClient.Client.ReadOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
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
			createRequest.Positions.Add(
				new PositionDto
				{
					Amount = 22,
					ProductId = Guid.NewGuid().ToString()
				});
			createRequest.Positions.Add(
				new PositionDto
				{
					Amount = 33,
					ProductId = Guid.NewGuid().ToString()
				});
			var createResponse = await GRpcClient.Client.CreateOrderAsync(
				createRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Equal(OrderStatusDto.Created, createResponse.OrderStatus);

			var readRequest = new OrderIdRequest
			{
				CustomerId = createRequest.CustomerId,
				OrderId = createResponse.OrderId
			};
			var readResponse = await GRpcClient.Client.ReadOrderAsync(
				readRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Equal(createResponse.OrderId, readResponse.OrderId);
			Assert.True(
				createRequest.Positions.All(
					requestPosition => createResponse.Positions.Any(
						position => position.Amount == requestPosition.Amount && position.ProductId == requestPosition.ProductId)));
			Assert.Equal(OrderStatusDto.Created, readResponse.OrderStatus);
			Assert.Equal(createRequest.CustomerId, readResponse.CustomerId);
		}
	}
}