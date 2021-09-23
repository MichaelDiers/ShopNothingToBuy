namespace Service.Contracts.Crud.Application
{
	/// <summary>
	///   Describes an application.
	/// </summary>
	public class ApplicationEntry : BaseApplicationEntry
	{
		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		public ApplicationEntry()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="id">The id of the application.</param>
		/// <param name="originalId">The original requested id at creation time.</param>
		/// <param name="roles">The available roles of the application.</param>
		public ApplicationEntry(string id, string originalId, Roles roles)
			: base(id, roles)
		{
			this.OriginalId = originalId;
		}

		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}