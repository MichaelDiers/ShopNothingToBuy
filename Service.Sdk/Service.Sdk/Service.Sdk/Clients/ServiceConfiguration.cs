namespace Service.Sdk.Clients
{
	/// <summary>
	///   The configuration for calling a service.
	/// </summary>
	public class ServiceConfiguration
	{
		/// <summary>
		///   Gets or sets the api key of the service.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		///   Gets or sets the url of the service.
		/// </summary>
		public string Url { get; set; }
	}
}