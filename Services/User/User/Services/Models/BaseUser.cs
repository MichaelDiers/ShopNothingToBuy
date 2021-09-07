namespace User.Services.Models
{
	/// <summary>
	///   User data that is used in <see cref="UserEntry" />, <see cref="CreateUserEntry" /> and <see cref="UpdateUserEntry" />
	///   .
	/// </summary>
	public abstract class BaseUser
	{
		/// <summary>
		///   Creates a new instance of <see cref="BaseUser" />.
		/// </summary>
		protected BaseUser()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="BaseUser" />.
		/// </summary>
		/// <param name="id">The id of the user.</param>
		/// <param name="applicationId">The id of the application the user is created for.</param>
		protected BaseUser(string id, string applicationId)
		{
			this.ApplicationId = applicationId;
			this.Id = id;
		}

		/// <summary>
		///   Gets or sets the id of the application the user is created for.
		/// </summary>
		public string ApplicationId { get; set; }

		/// <summary>
		///   Gets or sets the name of the user.
		/// </summary>
		public string Id { get; set; }
	}
}