namespace AuthApi.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using AuthApi.Contracts;

	/// <summary>
	///   Data-transfer object for accounts.
	/// </summary>
	public class AccountDto : Authentication
	{
		/// <summary>
		///   Create a new instance of <see cref="AccountDto" />.
		/// </summary>
		public AccountDto()
		{
		}


		/// <summary>
		///   Creates a new instance of <see cref="AccountDto" />.
		/// </summary>
		/// <param name="account">Data is initialized from given account.</param>
		public AccountDto(IAccount account)
		{
			this.IsLocked = account.IsLocked;
			this.Name = account.Name;
			this.Claims = account.Claims.Select(claim => new ClaimDto(claim)).ToArray();
		}

		/// <summary>
		///   Gets or sets the claims of the account.
		/// </summary>
		[Required]
		[MinLength(1)]
		[MaxLength(50)]
		public IReadOnlyCollection<ClaimDto> Claims { get; set; }

		/// <summary>
		///   Gets or sets the locked status of the account.
		/// </summary>
		[Required]
		public bool? IsLocked { get; set; }
	}
}