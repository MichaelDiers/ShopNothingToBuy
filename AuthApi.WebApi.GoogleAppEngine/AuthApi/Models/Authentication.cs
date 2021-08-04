namespace AuthApi.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///   Describes an authentication request.
	/// </summary>
	public class Authentication
	{
		/// <summary>
		///   Max length of account names.
		/// </summary>
		public const int NameMaxLength = 50;

		/// <summary>
		///   Min length for account names.
		/// </summary>
		public const int NameMinLength = 5;

		/// <summary>
		///   Max length for passwords.
		/// </summary>
		public const int PasswordMaxLength = 50;

		/// <summary>
		///   Min length for passwords.
		/// </summary>
		public const int PasswordMinLength = 8;

		/// <summary>
		///   Gets or sets the name of the account to authenticate.
		/// </summary>
		[Required]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; }

		/// <summary>
		///   Gets or sets the password of the account to authenticate.
		/// </summary>
		[Required]
		[MinLength(PasswordMinLength)]
		[MaxLength(PasswordMaxLength)]
		public string Password { get; set; }
	}
}