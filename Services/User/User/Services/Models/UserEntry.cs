namespace User.Services.Models
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes an user entry.
	/// </summary>
	public class UserEntry : IEntry<string>
	{
		/// <summary>
		///   Creates a new instance of <see cref="UserEntry" />.
		/// </summary>
		/// <param name="id">The id of the user.</param>
		public UserEntry(string id)
		{
			this.Id = id;
		}

		/// <summary>
		///   Gets the id of the user.
		/// </summary>
		public string Id { get; }
	}
}