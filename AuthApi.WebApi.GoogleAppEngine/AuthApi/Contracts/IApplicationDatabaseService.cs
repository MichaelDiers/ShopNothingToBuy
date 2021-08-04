namespace AuthApi.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	///   Describes database operations on <see cref="IApplication" /> instances.
	/// </summary>
	public interface IApplicationDatabaseService
	{
		/// <summary>
		///   Create a new application.
		/// </summary>
		/// <param name="application">The application data.</param>
		/// <returns>True if the application is created and false otherwise.</returns>
		Task<bool> Create(IApplication application);

		/// <summary>
		///   Delete an application by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>True if the application is deleted and false otherwise.</returns>
		Task<bool> DeleteApplication(string applicationId);

		/// <summary>
		///   Delete all applications.
		/// </summary>
		/// <returns>True if the operations succeeds and false otherwise.</returns>
		Task<bool> DeleteApplications();

		/// <summary>
		///   Read an application by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>An instance of <see cref="IApplication" /> or null if no application with given id exists.</returns>
		Task<IApplication> ReadApplication(string applicationId);

		/// <summary>
		///   Read all applications.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IApplication" />.</returns>
		Task<IEnumerable<IApplication>> ReadApplications();
	}
}