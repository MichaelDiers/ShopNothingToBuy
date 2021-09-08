namespace User.Services.Models
{
	using Application.Contracts;

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
		/// <param name="roles">The roles of the user.</param>
		protected BaseUser(string id, string applicationId, Roles roles)
		{
			this.ApplicationId = applicationId;
			this.Id = id;
			this.Roles = roles;
		}

		/// <summary>
		///   Gets or sets the id of the application the user is created for.
		/// </summary>
		public string ApplicationId { get; set; }

		/// <summary>
		///   Gets or sets the user id.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///   Gets or sets the roles of the user.
		/// </summary>
		public Roles Roles { get; set; }
	}
}