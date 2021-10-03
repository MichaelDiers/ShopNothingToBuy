namespace Service.Sdk.Clients.Crud
{
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Application;

	/// <summary>
	///   Client for <see cref="IApplicationService" />.
	/// </summary>
	public class ApplicationClient : Client<ApplicationEntry, string, CreateApplicationEntry, UpdateApplicationEntry>,
		IApplicationService
	{
		/// <summary>
		///   Creates a new instance of <see cref="ApplicationClient" />.
		/// </summary>
		/// <param name="logger">A logger for errors.</param>
		/// <param name="requestUrl">The url of the service.</param>
		public ApplicationClient(ILogger logger, string requestUrl)
			: base(logger, requestUrl)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="ApplicationClient" />.
		/// </summary>
		/// <param name="logger">A logger for errors.</param>
		/// <param name="apiKey">The api key for the application service.</param>
		/// <param name="requestUrl">The url of the service.</param>
		public ApplicationClient(ILogger logger, string apiKey, string requestUrl)
			: base(logger, apiKey, requestUrl)
		{
		}
	}
}