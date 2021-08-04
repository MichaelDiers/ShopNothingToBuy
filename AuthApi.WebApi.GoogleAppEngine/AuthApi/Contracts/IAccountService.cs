namespace AuthApi.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	///   Describes the operations on accounts.
	/// </summary>
	public interface IAccountService
	{
		/// <summary>
		///   Create a new <see cref="IAccount" />.
		/// </summary>
		/// <param name="account">The data for the account.</param>
		/// <returns>
		///   A <see cref="ValueTuple" /> of <see cref="IAccount" />, a bool indicating if the account already exists and a
		///   bool indicating if the data is valid.
		/// </returns>
		Task<(IAccount account, bool isConflict, bool isBadRequest)> Create(IAccount account);

		/// <summary>
		///   Delete all accounts of the application.
		/// </summary>
		/// <returns>True if all accounts are deleted and false otherwise.</returns>
		Task<bool> Delete();

		/// <summary>
		///   Delete an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The account to be deleted.</param>
		/// <returns>True if the account is deleted and false otherwise.</returns>
		Task<bool> Delete(string accountName);

		/// <summary>
		///   Read an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The name of the account.</param>
		/// <returns>An instance of <see cref="IAccount" /> or null if no account with name <paramref name="accountName" /> exists.</returns>
		Task<IAccount> Read(string accountName);

		/// <summary>
		///   Read all accounts of the application.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IAccount" />.</returns>
		Task<IEnumerable<IAccount>> Read();

		/// <summary>
		///   Update an account.
		/// </summary>
		/// <param name="account">The new account data.</param>
		/// <returns>True if an account with <see cref="IAccount.Name" /> exists and the update is executed and false otherwise.</returns>
		Task<bool> Update(IAccount account);
	}
}