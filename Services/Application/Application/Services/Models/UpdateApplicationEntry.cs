namespace Application.Services.Models
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes the data for updating an application.
	/// </summary>
	public class UpdateApplicationEntry : BaseApplicationEntry, IEntry<string>
	{
		/// <summary>
		///   Gets or sets the id of the application.
		/// </summary>
		public string Id { get; set; }
	}
}