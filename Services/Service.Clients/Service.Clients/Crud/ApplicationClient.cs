namespace Service.Clients.Crud
{
	using Service.Contracts.Business.Log;
	using Service.Contracts.Crud.Application;
	using Service.Models.Crud.Application;
	using Service.Sdk.Clients;

	/// <summary>
	///   Client for the <see cref="IApplicationService" />.
	/// </summary>
	public class ApplicationClient : Client<ApplicationEntry, IApplicationEntry, string, ICreateApplicationEntry,
			IUpdateApplicationEntry>,
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