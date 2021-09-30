namespace Service.Sdk.Contracts.Crud.User
{
	using System.Collections.Generic;
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Describes an user entry.
	/// </summary>
	public class UserEntry : BaseUserEntry, IEntry<string>
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
		/// <param name="id">The id of the user entry.</param>
		/// <param name="originalId">The original id of the user entry.</param>
		/// <param name="userApplicationEntries">The application and roles that the use has permissions for.</param>
		public UserEntry(string id, string originalId, IEnumerable<UserApplicationEntry> userApplicationEntries)
			: base(id, userApplicationEntries)
		{
			this.OriginalId = originalId;
		}

		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}