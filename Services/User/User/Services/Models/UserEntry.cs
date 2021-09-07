namespace User.Services.Models
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes an user entry.
	/// </summary>
	public class UserEntry : BaseUser, IEntry<string>
	{
		/// <summary>
		///   Creates a new instance of <see cref="UserEntry" />.
		/// </summary>
		public UserEntry()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="UserEntry" />.
		/// </summary>
		/// <param name="createUserEntry">Data is initialized from the given entry.</param>
		public UserEntry(CreateUserEntry createUserEntry)
		{
			this.Id = createUserEntry.Id;
			this.ApplicationId = createUserEntry.ApplicationId;
		}

		/// <summary>
		///   Creates a new instance of <see cref="UserEntry" />.
		/// </summary>
		/// <param name="updateUserEntry">Data is initialized from the given entry.</param>
		public UserEntry(UpdateUserEntry updateUserEntry)
		{
			this.Id = updateUserEntry.Id;
			this.ApplicationId = updateUserEntry.ApplicationId;
		}
	}
}