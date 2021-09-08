namespace Application.Contracts
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes the data for updating an application.
	/// </summary>
	public class UpdateApplicationEntry : BaseApplicationEntry, IEntry<string>
	{
		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}