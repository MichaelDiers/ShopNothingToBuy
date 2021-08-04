namespace AuthApi.Contracts
{
	using System.Collections.Generic;
	using AuthApi.Models;

	/// <summary>
	///   Describes an application.
	/// </summary>
	public interface IApplication
	{
		/// <summary>
		///   Gets a value used as an id for the application.
		/// </summary>
		string Id { get; }

		/// <summary>
		///   Gets a value for the name of the application.
		/// </summary>
		string Name { get; }

		/// <summary>
		///   Gets an <see cref="IReadOnlyCollection{T}" /> of <see cref="Role" /> used as the roles of the application.
		/// </summary>
		IReadOnlyCollection<Role> Roles { get; }
	}
}