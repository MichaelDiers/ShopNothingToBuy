namespace Service.Sdk.Clients
{
	using Service.Contracts.Client;

	/// <summary>
	///   A result of a service operation that includes response data.
	/// </summary>
	/// <typeparam name="TResponseData">The type of the response data.</typeparam>
	public class ClientResult<TResponseData> : EmptyClientResult, IClientResult<TResponseData>
	{
		/// <summary>
		///   Gets or sets the response data from the body.
		/// </summary>
		public TResponseData ResponseData { get; set; }
	}
}