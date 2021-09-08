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
		///   Gets or sets the id of the application.
		/// </summary>
		public string Id { get; set; }
	}
}