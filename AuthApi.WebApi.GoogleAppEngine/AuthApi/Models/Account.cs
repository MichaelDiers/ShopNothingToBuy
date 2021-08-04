namespace AuthApi.Models
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Security.Claims;
	using AuthApi.Contracts;
	using AuthApi.Converter;
	using Google.Cloud.Firestore;

	/// <summary>
	///   Describes an account.
	/// </summary>
	[FirestoreData]
	public class Account : IAccount
	{
		/// <summary>
		///   Creates a new instance of <see cref="Account" />.
		/// </summary>
		public Account()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="Account" />.
		/// </summary>
		/// <param name="accountDto">Values are initialized from the given account.</param>
		public Account(AccountDto accountDto)
		{
			if (accountDto == null)
			{
				throw new ArgumentNullException(nameof(accountDto));
			}

			if (string.IsNullOrWhiteSpace(accountDto.Name))
			{
				throw new ArgumentNullException(nameof(accountDto.Name));
			}

			if (string.IsNullOrWhiteSpace(accountDto.Password))
			{
				throw new ArgumentNullException(nameof(accountDto.Password));
			}

			this.IsLocked = accountDto.IsLocked ?? throw new ArgumentNullException(nameof(accountDto.IsLocked));
			this.Name = accountDto.Name;
			this.Password = accountDto.Password;
			this.Claims = new ReadOnlyCollection<Claim>(
				accountDto.Claims.Select(claim => new Claim(claim.Type, claim.Value)).ToArray());
		}

		/// <summary>
		///   Gets or sets the <see cref="Claim" />s of the account.
		/// </summary>
		[FirestoreProperty("claims", ConverterType = typeof(ClaimCollectionFirestoreConverter))]
		public IReadOnlyCollection<Claim> Claims { get; set; }

		/// <summary>
		///   Gets or sets a value that specifies if the account is locked.
		/// </summary>
		[FirestoreProperty("isLocked")]
		public bool IsLocked { get; set; }

		/// <summary>
		///   Gets or sets the name of the account.
		/// </summary>
		[FirestoreProperty("name")]
		public string Name { get; set; }

		/// <summary>
		///   Gets or sets the password of the account.
		/// </summary>
		[FirestoreProperty("password")]
		public string Password { get; set; }
	}
}