namespace Application.Clients
{
	using Service.Sdk.Clients;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Application;

	/// <summary>
	///   Client for the <see cref="IApplicationService" />.
	/// </summary>
	public class ApplicationClient : Client<ApplicationEntry, string, CreateApplicationEntry, UpdateApplicationEntry>,
		IApplicationService
	{
		/// <summary>
		///   Creates a new instance of <see cref="ApplicationClient" />.
		/// </summary>
		/// <param name="logger">A logger for error messages.</param>
		/// <param name="apiKey">The api key used for service calls.</param>
		/// <param name="requestUrl">The url of the service.</param>
		public ApplicationClient(ILogger logger, string apiKey, string requestUrl)
			: base(logger, apiKey, requestUrl)
		{
		}
	}
}