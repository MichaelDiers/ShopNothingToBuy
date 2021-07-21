namespace OrdersApi.Server.Extensions
{
	using Grpc.Core;

	/// <summary>
	///   Extensions for <see cref="ServerCallContext" />.
	/// </summary>
	public static class ServerCallContextExtensions
	{
		/// <summary>
		///   Get the specified value from the <see cref="ServerCallContext.UserState" /> and cast to the requested type.
		/// </summary>
		/// <typeparam name="T">The type of the requested value.</typeparam>
		/// <param name="context">
		///   The <see cref="ServerCallContext" /> that <see cref="ServerCallContext.UserState" /> should
		///   contain the <paramref name="key" />.
		/// </param>
		/// <param name="key">The name of the requested value in <see cref="ServerCallContext.UserState" />.</param>
		/// <returns>The value of the requested <paramref name="key" />.</returns>
		public static T Get<T>(this ServerCallContext context, string key)
		{
			return (T) context.UserState[key];
		}
	}
}