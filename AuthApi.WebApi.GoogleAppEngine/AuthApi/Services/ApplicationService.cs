namespace AuthApi.Services
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using AuthApi.Contracts;

	/// <summary>
	///   Describes operations on <see cref="IApplication" /> instances.
	/// </summary>
	public class ApplicationService : IApplicationService
	{
		/// <summary>
		///   Service for database operations.
		/// </summary>
		private readonly IApplicationDatabaseService databaseService;

		/// <summary>
		///   Creates a new instance of <see cref="ApplicationService" />.
		/// </summary>
		/// <param name="databaseService">Service for database operations on applications.</param>
		public ApplicationService(IApplicationDatabaseService databaseService)
		{
			this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
		}

		/// <summary>
		///   Create a new application.
		/// </summary>
		/// <param name="application">The data of the application.</param>
		/// <returns>An instance of <see cref="IApplication" />.</returns>
		public async Task<IApplication> Create(IApplication application)
		{
			return await this.databaseService.Create(application) ? application : null;
		}

		/// <summary>
		///   Delete an application by its id.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		public async Task<bool> Delete(string applicationId)
		{
			return await this.databaseService.DeleteApplication(applicationId);
		}

		/// <summary>
		///   Delete an application.
		/// </summary>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		public async Task<bool> DeleteApplications()
		{
			return await this.databaseService.DeleteApplications();
		}

		/// <summary>
		///   Read an application by its id.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>An <see cref="IApplication" /> or null if the application does not exist.</returns>
		public async Task<IApplication> ReadApplication(string applicationId)
		{
			return await this.databaseService.ReadApplication(applicationId);
		}

		/// <summary>
		///   Read all applications.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> ot <see cref="IApplication" />.</returns>
		public async Task<IEnumerable<IApplication>> ReadApplications()
		{
			return await this.databaseService.ReadApplications();
		}
	}
}