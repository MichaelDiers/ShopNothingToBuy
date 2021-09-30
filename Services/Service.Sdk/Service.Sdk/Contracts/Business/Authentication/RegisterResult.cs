namespace Service.Sdk.Contracts.Business.Authentication
{
	/// <summary>
	///   Specifies the registration result.
	/// </summary>
	public enum RegisterResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   New user is registered.
		/// </summary>
		Registered = 1,

		/// <summary>
		///   User already exists.
		/// </summary>
		UserExists = 2,

		/// <summary>
		///   User name validation failed.
		/// </summary>
		UserInvalid = 3,

		/// <summary>
		///   Password validation failed.
		/// </summary>
		PasswordInvalid = 4,

		/// <summary>
		///   An unexpected error occurred.
		/// </summary>
		InternalError = 5
	}
}