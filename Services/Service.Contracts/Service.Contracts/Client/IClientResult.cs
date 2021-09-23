namespace Service.Contracts.Client
{
	/// <summary>
	///   A result of a service operation that includes response data.
	/// </summary>
	/// <typeparam name="TResponseData">The type of the response data.</typeparam>
	public interface IClientResult<out TResponseData> : IEmptyClientResult
	{
		/// <summary>
		///   Gets the response data from the body.
		/// </summary>
		public TResponseData ResponseData { get; }
	}
}