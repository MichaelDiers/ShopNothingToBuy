namespace OrdersApi.IntegrationTests.Tests
{
	using System;
	using System.Linq;
	using Grpc.Core;
	using OrdersApi.Server;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="Orders.OrdersClient.CreateOrderAsync(CreateOrderRequest,CallOptions)" />.
	/// </summary>
	[Collection("Sequential")]
	public class CreateOrderTests
	{
		[Fact]
		public async void ShouldFailIfApiKeyIsInvalid()
		{
			var request = new CreateOrderRequest();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(Guid.NewGuid().ToString())));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsMissing()
		{
			var request = new CreateOrderRequest();
			await TestHelper.CheckStatusCode(async () => await GRpcClient.Client.CreateOrderAsync(request));
		}

		[Fact]
		public async void ShouldFailIfApiKeyIsNotAGuid()
		{
			var request = new CreateOrderRequest();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions("notAGuid")));
		}

		[Fact]
		public async void ShouldFailIfEmptyPositionIsIncluded()
		{
			var request = new CreateOrderRequest();
			request.Positions.Add(new PositionDto());
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(CreateOrderRequest.Positions));
		}

		[Fact]
		public async void ShouldFailIfProductIdMissingPositionIsIncluded()
		{
			var request = new CreateOrderRequest();
			request.Positions.Add(
				new PositionDto
				{
					Amount = 1
				});
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(CreateOrderRequest.Positions));
		}

		[Fact]
		public async void ShouldFailIfProductIdNoGuidPositionIsIncluded()
		{
			var request = new CreateOrderRequest();
			request.Positions.Add(
				new PositionDto
				{
					Amount = 1,
					ProductId = "noGuid"
				});
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(CreateOrderRequest.Positions));
		}

		[Fact]
		public async void ShouldFailIfZeroAmountPositionIsIncluded()
		{
			var request = new CreateOrderRequest();
			request.Positions.Add(
				new PositionDto
				{
					Amount = 0,
					ProductId = Guid.NewGuid().ToString()
				});
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(CreateOrderRequest.Positions));
		}

		[Fact]
		public async void ShouldFailWithoutPositions()
		{
			var request = new CreateOrderRequest();
			await TestHelper.CheckStatusCode(
				async () =>
					await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey)),
				StatusCode.InvalidArgument,
				nameof(CreateOrderRequest.Positions));
		}

		[Fact]
		public async void ShouldSucceedForValidMultiplePositions()
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
			request.Positions.Add(
				new PositionDto
				{
					Amount = 234,
					ProductId = Guid.NewGuid().ToString()
				});

			var response = await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.False(string.IsNullOrWhiteSpace(response.OrderId));
			Assert.Equal(OrderStatusDto.Created, response.OrderStatus);
			Assert.Equal(2, response.Positions.Count);
			foreach (var requestPosition in request.Positions)
			{
				Assert.Single(
					response.Positions.Where(
						position => position.Amount == requestPosition.Amount && position.ProductId == requestPosition.ProductId));
			}
		}

		[Fact]
		public async void ShouldSucceedForValidSinglePosition()
		{
			const uint amount = 1;
			var productId = Guid.NewGuid().ToString();
			var request = new CreateOrderRequest
			{
				CustomerId = Guid.NewGuid().ToString()
			};

			request.Positions.Add(
				new PositionDto
				{
					Amount = amount,
					ProductId = productId
				});
			var response = await GRpcClient.Client.CreateOrderAsync(request, GRpcClient.CallOptions(TestHelper.ApiKey));
			Assert.False(string.IsNullOrWhiteSpace(response.OrderId));
			Assert.Equal(OrderStatusDto.Created, response.OrderStatus);
			Assert.Single(response.Positions);
			Assert.Equal(response.Positions[0].Amount, amount);
			Assert.Equal(response.Positions[0].ProductId, productId);
		}
	}
}