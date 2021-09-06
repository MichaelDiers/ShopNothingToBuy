namespace User.Services.Models
{
	/// <summary>
	///   User data that is used in <see cref="UserEntry" />, <see cref="CreateUserEntry" /> and <see cref="UpdateUserEntry" />
	///   .
	/// </summary>
	public class BaseUser
	{
		/// <summary>
		///   Creates a new instance of <see cref="BaseUser" />.
		/// </summary>
		public BaseUser()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="BaseUser" />.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="applicationId">The id of the application the user is created for.</param>
		public BaseUser(string name, string applicationId)
		{
			this.ApplicationId = applicationId;
			this.Name = name;
		}

		/// <summary>
		///   Gets or sets the id of the application the user is created for.
		/// </summary>
		public string ApplicationId { get; set; }

		/// <summary>
		///   Gets or sets the name of the user.
		/// </summary>
		public string Name { get; set; }
	}
}