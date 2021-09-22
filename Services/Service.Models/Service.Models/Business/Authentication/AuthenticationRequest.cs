namespace Service.Models.Business.Authentication
{
	using Service.Contracts.Business.Authentication;

	/// <summary>
	///   Describes an authentication request.
	/// </summary>
	public class AuthenticationRequest : IAuthenticationRequest
	{
		/// <summary>
		///   Gets or sets the password of the user.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///   Gets or sets the name of the user.
		/// </summary>
		public string UserName { get; set; }
	}
}