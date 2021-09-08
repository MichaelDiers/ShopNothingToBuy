namespace Application.Contracts
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes an application.
	/// </summary>
	public class ApplicationEntry : BaseApplicationEntry, IEntry<string>
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
		public ApplicationEntry(string id, string originalId)
		{
			this.Id = id;
			this.OriginalId = originalId;
		}

		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="createApplicationEntry">Data is initialized from the given entry.</param>
		public ApplicationEntry(CreateApplicationEntry createApplicationEntry)
			: this(createApplicationEntry?.Id?.ToUpper(), createApplicationEntry?.Id)
		{
		}

		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="updateApplicationEntry">Data is initialized from the given entry.</param>
		public ApplicationEntry(UpdateApplicationEntry updateApplicationEntry)
			: this(updateApplicationEntry?.Id?.ToUpper(), updateApplicationEntry?.OriginalId)
		{
		}

		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}