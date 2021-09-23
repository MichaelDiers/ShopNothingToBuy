namespace Service.Contracts.Crud.Application
{
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes the base data of the application.
	/// </summary>
	public class BaseApplicationEntry : Entry<string>
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
		///   Gets or sets the available roles for the application.
		/// </summary>
		public Roles Roles { get; set; }
	}
}