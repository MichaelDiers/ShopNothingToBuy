namespace User.Services.Models
{
	using System;
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
			this.Id = Guid.NewGuid().ToString();
			this.Name = createUserEntry.Name;
		}

		/// <summary>
		///   Creates a new instance of <see cref="UserEntry" />.
		/// </summary>
		/// <param name="updateUserEntry">Data is initialized from the given entry.</param>
		public UserEntry(UpdateUserEntry updateUserEntry)
		{
			this.Id = updateUserEntry.Id;
			this.Name = updateUserEntry.Name;
		}

		/// <summary>
		///   Gets or sets the id of the user.
		/// </summary>
		public string Id { get; set; }
	}
}