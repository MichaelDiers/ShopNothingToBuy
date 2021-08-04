namespace AuthApi.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using AuthApi.Contracts;
	using BCrypt.Net;

	/// <summary>
	///   Describes the operations on accounts.
	/// </summary>
	public class AccountService : IAccountService
	{
		/// <summary>
		///   Service for database operations.
		/// </summary>
		private readonly IAccountDatabaseService databaseService;

		/// <summary>
		///   Creates a new instance of <see cref="AccountService" />.
		/// </summary>
		/// <param name="databaseService">Service for database operations on <see cref="IApplication" /> instances.</param>
		public AccountService(IAccountDatabaseService databaseService)
		{
			this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
		}

		/// <summary>
		///   Create a new <see cref="IAccount" />.
		/// </summary>
		/// <param name="account">The data for the account.</param>
		/// <returns>
		///   A <see cref="ValueTuple" /> of <see cref="IAccount" />, a bool indicating if the account already exists and a
		///   bool indicating if the data is valid.
		/// </returns>
		public async Task<(IAccount account, bool isConflict, bool isBadRequest)> Create(IAccount account)
		{
			// cross check if all applications and roles do exist
			var applications =
				(await this.databaseService.ReadApplications(
					account.Claims.Select(claim => claim.Type).Distinct()
						.ToArray()))
				.ToArray();

			foreach (var accountClaim in account.Claims)
			{
				var application = applications.FirstOrDefault(app => app.Id.ToString() == accountClaim.Type);
				if (application is null)
				{
					return (null, false, true);
				}

				if (application.Roles.All(role => role.Id.ToString() != accountClaim.Value))
				{
					return (null, false, true);
				}
			}

			account.Password = BCrypt.HashPassword(account.Password);
			return await this.databaseService.Create(account) ? (account, false, false) : (null, true, false);
		}

		/// <summary>
		///   Delete an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The account to be deleted.</param>
		/// <returns>True if the account is deleted and false otherwise.</returns>
		public async Task<bool> Delete(string accountName)
		{
			return await this.databaseService.DeleteAccount(accountName);
		}

		/// <summary>
		///   Delete all accounts of the application.
		/// </summary>
		/// <returns>True if all accounts are deleted and false otherwise.</returns>
		public async Task<bool> Delete()
		{
			return await this.databaseService.DeleteAccounts();
		}

		/// <summary>
		///   Read an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The name of the account.</param>
		/// <returns>An instance of <see cref="IAccount" /> or null if no account with name <paramref name="accountName" /> exists.</returns>
		public async Task<IAccount> Read(string accountName)
		{
			var account = await this.databaseService.ReadAccount(accountName);
			return account;
		}

		/// <summary>
		///   Read all accounts of the application.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IAccount" />.</returns>
		public async Task<IEnumerable<IAccount>> Read()
		{
			return await this.databaseService.ReadAccounts();
		}

		/// <summary>
		///   Update an account.
		/// </summary>
		/// <param name="account">The new account data.</param>
		/// <returns>True if an account with <see cref="IAccount.Name" /> exists and the update is executed and false otherwise.</returns>
		public async Task<bool> Update(IAccount account)
		{
			var accountDatabase = await this.databaseService.ReadAccount(account.Name);
			if (accountDatabase == null)
			{
				return false;
			}

			account.Password = string.IsNullOrWhiteSpace(account.Password)
				? accountDatabase.Password
				: BCrypt.HashPassword(account.Password);

			return await this.databaseService.Update(account);
		}
	}
}