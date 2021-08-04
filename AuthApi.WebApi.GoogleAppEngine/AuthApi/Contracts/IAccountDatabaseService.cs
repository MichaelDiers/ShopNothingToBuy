namespace AuthApi.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	///   Specifies database operations on <see cref="IAccount" /> instances.
	/// </summary>
	public interface IAccountDatabaseService
	{
		/// <summary>
		///   Create a new account.
		/// </summary>
		/// <param name="account">The account to be created.</param>
		/// <returns>True if the account is created and otherwise false.</returns>
		Task<bool> Create(IAccount account);

		/// <summary>
		///   Delete an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The account to be deleted.</param>
		/// <returns>True if the account is deleted and false otherwise.</returns>
		Task<bool> DeleteAccount(string accountName);

		/// <summary>
		///   Delete all accounts of the application.
		/// </summary>
		/// <returns></returns>
		Task<bool> DeleteAccounts();

		/// <summary>
		///   Read an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The name of the account.</param>
		/// <returns>An instance of <see cref="IAccount" /> or null if not found.</returns>
		Task<IAccount> ReadAccount(string accountName);

		/// <summary>
		///   Read all accounts from the database.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IAccount" />.</returns>
		Task<IEnumerable<IAccount>> ReadAccounts();

		/// <summary>
		///   Read applications by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="applicationIds">The ids of the requested applications.</param>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IApplication" />.</returns>
		Task<IEnumerable<IApplication>> ReadApplications(IReadOnlyCollection<string> applicationIds);

		/// <summary>
		///   Replace the database document <see cref="IAccount.Name" /> by the contents of <paramref name="account" />.
		/// </summary>
		/// <param name="account">The new values for the account. The <see cref="IAccount.Name" /> is used as unique reference.</param>
		/// <returns>True if the account is updated and false otherwise.</returns>
		Task<bool> Update(IAccount account);
	}
}