namespace Service.Sdk.Contracts.Business.Authentication
{
	/// <summary>
	///   Describes an authentication request.
	/// </summary>
	public interface IAuthenticationRequest
	{
		/// <summary>
		///   Gets the password of the user.
		/// </summary>
		public string Password { get; }

		/// <summary>
		///   Gets the name of the user.
		/// </summary>
		public string UserName { get; }
	}
}