namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using Grpc.Core;
	using OrdersApi.Server;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="Orders.OrdersClient.UpdateStatusAsync(UpdateStatusRequest,CallOptions)" />.
	/// </summary>
	[Collection("Sequential")]
	public class UpdateStatusTests
	{
		[Fact]
		public async void ShouldFailForUnknownCustomerId()
		{
			var request = new CreateOrderRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			request.Positions.Add(
				new PositionDto
				{
					Amount = 1,
					ProductId = Guid.NewGuid().ToString()
				});
			var response = await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.NotNull(response);
			Assert.False(string.IsNullOrWhiteSpace(response.OrderId));

			var updateRequest = new UpdateStatusRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = response.OrderId,
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.UpdateStatusAsync(
						updateRequest,
						GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
		}

		[Fact]
		public async void ShouldFailForUnknownOrderId()
		{
			var request = new CreateOrderRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			request.Positions.Add(
				new PositionDto
				{
					Amount = 1,
					ProductId = Guid.NewGuid().ToString()
				});
			var response = await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.NotNull(response);
			Assert.False(string.IsNullOrWhiteSpace(response.OrderId));

			var updateRequest = new UpdateStatusRequest
			{
				CustomerId = request.CustomerId,
				OrderId = Guid.NewGuid().ToString(),
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.UpdateStatusAsync(
						updateRequest,
						GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.NotFound);
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsInvalid()
		{
			var request = new UpdateStatusRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = Guid.NewGuid().ToString(),
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.UpdateStatusAsync(request, GRpcClient.CallOptions(Guid.NewGuid().ToString())));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsMissing()
		{
			var request = new UpdateStatusRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = Guid.NewGuid().ToString(),
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(async () => await GRpcClient.Client.UpdateStatusAsync(request));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsNotAGuid()
		{
			var request = new UpdateStatusRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = Guid.NewGuid().ToString(),
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.UpdateStatusAsync(request, GRpcClient.CallOptions("notAGuid")));
		}

		[Fact]
		public async void ShouldFailIfCustomerIdIsMissing()
		{
			var request = new UpdateStatusRequest
			{
				OrderId = Guid.NewGuid().ToString(),
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.UpdateStatusAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(UpdateStatusRequest.CustomerId));
		}

		[Fact]
		public async void ShouldFailIfCustomerIdIsNotAGuid()
		{
			var request = new UpdateStatusRequest
			{
				CustomerId = "notAGuid",
				OrderId = Guid.NewGuid().ToString(),
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.UpdateStatusAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(UpdateStatusRequest.CustomerId));
		}

		[Fact]
		public async void ShouldFailIfOrderIdIsMissing()
		{
			var request = new UpdateStatusRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.UpdateStatusAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(UpdateStatusRequest.OrderId));
		}

		[Fact]
		public async void ShouldFailIfOrderIdIsNotAGuid()
		{
			var request = new UpdateStatusRequest
			{
				CustomerId = Guid.NewGuid().ToString(),
				OrderId = "notAGuid",
				NewStatus = OrderStatusDto.Rejected
			};

			await TestHelper.CheckStatusCode(
				async () => await GRpcClient.Client.UpdateStatusAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(UpdateStatusRequest.OrderId));
		}

		[Fact]
		public async void ShouldSucceed()
		{
			var request = new CreateOrderRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			request.Positions.Add(
				new PositionDto
				{
					Amount = 1,
					ProductId = Guid.NewGuid().ToString()
				});
			var response = await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.NotNull(response);
			Assert.False(string.IsNullOrWhiteSpace(response.OrderId));

			var updateRequest = new UpdateStatusRequest
			{
				CustomerId = request.CustomerId,
				OrderId = response.OrderId,
				NewStatus = OrderStatusDto.Rejected
			};

			var updateResponse = await GRpcClient.Client.UpdateStatusAsync(
				updateRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.NotNull(updateResponse);

			var readRequest = new OrderIdRequest
			{
				CustomerId = request.CustomerId,
				OrderId = response.OrderId
			};

			var readResponse = await GRpcClient.Client.ReadOrderAsync(
				readRequest,
				GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.Equal(OrderStatusDto.Rejected, readResponse.OrderStatus);
		}
	}
}