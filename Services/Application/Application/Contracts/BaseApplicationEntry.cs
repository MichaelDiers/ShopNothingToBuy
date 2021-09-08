namespace Application.Contracts
{
	/// <summary>
	///   Describes the base data of the application.
	/// </summary>
	public class BaseApplicationEntry
	{
		/// <summary>
		///   Creates a new instance of <see cref="BaseApplicationEntry" />.
		/// </summary>
		protected BaseApplicationEntry()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="BaseApplicationEntry" />.
		/// </summary>
		/// <param name="id">The id of the application.</param>
		/// <param name="roles">The available roles.</param>
		protected BaseApplicationEntry(string id, Roles roles)
		{
			this.Id = id;
			this.Roles = roles;
		}

		/// <summary>
		///   Gets or sets the id of the application.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///   Gets or sets the available roles for the application.
		/// </summary>
		public Roles Roles { get; set; }
	}
}