namespace AuthApi.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	///   Describes operations on <see cref="IApplication" /> instances.
	/// </summary>
	public interface IApplicationService
	{
		/// <summary>
		///   Create a new application.
		/// </summary>
		/// <param name="application">The data of the application.</param>
		/// <returns>An instance of <see cref="IApplication" />.</returns>
		Task<IApplication> Create(IApplication application);

		/// <summary>
		///   Delete an application by its id.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		Task<bool> Delete(string applicationId);

		/// <summary>
		///   Delete an application.
		/// </summary>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		Task<bool> DeleteApplications();

		/// <summary>
		///   Read an application by its id.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>An <see cref="IApplication" /> or null if the application does not exist.</returns>
		Task<IApplication> ReadApplication(string applicationId);

		/// <summary>
		///   Read all applications.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> ot <see cref="IApplication" />.</returns>
		Task<IEnumerable<IApplication>> ReadApplications();
	}
}