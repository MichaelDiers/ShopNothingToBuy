namespace OrdersApi.Server.Models
{
	/// <summary>
	///   Specifies the status of an <see cref="Order" />.
	/// </summary>
	public enum OrderStatus
	{
		/// <summary>
		///   Undefined state.
		/// </summary>
		None = 0,

		/// <summary>
		///   The order is created and not yet processed.
		/// </summary>
		Created = 1,

		/// <summary>
		///   The order is rejected by the service.
		/// </summary>
		Rejected = 2
	}
}