namespace Service.Sdk.Contracts.Crud.User
{
	using Service.Sdk.Contracts.Crud.Application;

	/// <summary>
	///   Specifies which roles a user owns for an application.
	/// </summary>
	public class UserApplicationEntry
	{
		/// <summary>
		///   Creates a new instance od <see cref="UserApplicationEntry" />
		/// </summary>
		public UserApplicationEntry()
		{
		}

		/// <summary>
		///   Creates a new instance od <see cref="UserApplicationEntry" />
		///   <param name="applicationId">The id of the application.</param>
		///   <param name="roles">The roles the user has access to.</param>
		/// </summary>
		public UserApplicationEntry(string applicationId, Roles roles)
		{
			this.ApplicationId = applicationId;
			this.Roles = roles;
		}

		/// <summary>
		///   Gets or sets the id of the application.
		/// </summary>
		public string ApplicationId { get; set; }

		/// <summary>
		///   Gets or sets the roles for the application.
		/// </summary>
		public Roles Roles { get; set; }
	}
}