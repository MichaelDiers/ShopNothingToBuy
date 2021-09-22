namespace Service.Models.Business.Authentication
{
	/// <summary>
	///   Base model for the authentication service.
	/// </summary>
	public class AuthenticationBase
	{
		/// <summary>
		///   Creates a new instance of <see cref="AuthenticationBase" />
		/// </summary>
		protected AuthenticationBase()
		{
		}

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