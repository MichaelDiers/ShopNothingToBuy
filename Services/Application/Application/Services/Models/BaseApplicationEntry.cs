namespace Application.Services.Models
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
		///   Gets or sets the name of the application.
		/// </summary>
		public string Name { get; set; }
	}
}