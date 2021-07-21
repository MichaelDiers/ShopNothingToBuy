namespace OrdersApi.Server.Extensions
{
	using Grpc.Core;
	using OrdersApi.Server.Models;

	/// <summary>
	///   Extensions for the <see cref="OrderStatusDto" /> enum.
	/// </summary>
	public static class OrderStatusDtoExtensions
	{
		/// <summary>
		///   Map an <see cref="OrderStatusDto" /> to an <see cref="OrderStatus" />.
		/// </summary>
		/// <param name="orderStatusDto">The <see cref="OrderStatusDto" /> to map.</param>
		/// <returns>A matching <see cref="OrderStatus" />.</returns>
		/// <exception cref="RpcException">If <paramref name="orderStatusDto" /> cannot be matched.</exception>
		public static OrderStatus ToOrderStatus(this OrderStatusDto orderStatusDto)
		{
			switch (orderStatusDto)
			{
				case OrderStatusDto.None:
					return OrderStatus.None;
				case OrderStatusDto.Created:
					return OrderStatus.Created;
				case OrderStatusDto.Rejected:
					return OrderStatus.Rejected;
				default:
					throw new RpcException(new Status(StatusCode.Internal, "Unknown status: " + orderStatusDto));
			}
		}
	}
}