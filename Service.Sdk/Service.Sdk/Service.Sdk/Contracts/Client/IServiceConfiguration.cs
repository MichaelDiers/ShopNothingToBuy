namespace Service.Sdk.Contracts.Client
{
	/// <summary>
	///   The configuration for calling a service.
	/// </summary>
	public interface IServiceConfiguration
	{
		/// <summary>
		///   Gets or sets the api key of the service.
		/// </summary>
		public string ApiKey { get; }

		/// <summary>
		///   Gets or sets the url of the service.
		/// </summary>
		public string Url { get; }
	}
}