namespace OrdersApi.Server.Extensions
{
	using Grpc.Core;
	using OrdersApi.Server.Models;

	/// <summary>
	///   Extensions for the <see cref="OrderStatus" /> enum.
	/// </summary>
	public static class OrderStatusExtensions
	{
		/// <summary>
		///   Map an <see cref="OrderStatus" /> to an <see cref="OrderStatusDto" />.
		/// </summary>
		/// <param name="orderStatus">The <see cref="OrderStatus" /> to map.</param>
		/// <returns>A matching <see cref="OrderStatusDto" />.</returns>
		/// <exception cref="RpcException">If <paramref name="orderStatus" /> cannot be matched.</exception>
		public static OrderStatusDto ToOrderStatusDto(this OrderStatus orderStatus)
		{
			switch (orderStatus)
			{
				case OrderStatus.None:
					return OrderStatusDto.None;
				case OrderStatus.Created:
					return OrderStatusDto.Created;
				case OrderStatus.Rejected:
					return OrderStatusDto.Rejected;
				default:
					throw new RpcException(new Status(StatusCode.Internal, "Unknown status: " + orderStatus));
			}
		}
	}
}