namespace AuthApi.Contracts
{
	using System.Collections.Generic;
	using System.Security.Claims;

	/// <summary>
	///   Describes an account.
	/// </summary>
	public interface IAccount
	{
		/// <summary>
		///   Gets the claims of the account.
		/// </summary>
		IReadOnlyCollection<Claim> Claims { get; }

		/// <summary>
		///   Gets a value indicating the account is locked.
		/// </summary>
		bool IsLocked { get; }

		/// <summary>
		///   Gets a value specifying the name of the account.
		/// </summary>
		string Name { get; }

		/// <summary>
		///   Gets or sets a value used as a password for the account.
		/// </summary>
		string Password { get; set; }
	}
}