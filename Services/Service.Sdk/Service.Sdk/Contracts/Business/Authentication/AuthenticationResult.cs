namespace Service.Sdk.Contracts.Business.Authentication
{
	/// <summary>
	///   Describes the result of an authentication request.
	/// </summary>
	public enum AuthenticationResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   Authentication did succeed.
		/// </summary>
		Authorized = 1,

		/// <summary>
		///   The combination of user and password is unknown.
		/// </summary>
		Failed = 2,

		/// <summary>
		///   An unexpected error occurred.
		/// </summary>
		InternalError = 3
	}
}