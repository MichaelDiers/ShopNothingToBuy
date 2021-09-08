namespace User.Services.Models
{
	using Application.Contracts;
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
			: base(createUserEntry?.Id.ToUpper(), createUserEntry?.ApplicationId, createUserEntry?.Roles ?? Roles.None)
		{
			this.OriginalId = createUserEntry?.Id;
		}

		/// <summary>
		///   Creates a new instance of <see cref="UserEntry" />.
		/// </summary>
		/// <param name="updateUserEntry">Data is initialized from the given entry.</param>
		public UserEntry(UpdateUserEntry updateUserEntry)
			: base(updateUserEntry?.Id?.ToUpper(), updateUserEntry?.ApplicationId, updateUserEntry?.Roles ?? Roles.None)
		{
			this.OriginalId = updateUserEntry?.OriginalId;
		}

		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}