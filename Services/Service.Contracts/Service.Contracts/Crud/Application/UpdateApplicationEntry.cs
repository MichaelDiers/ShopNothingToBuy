namespace Service.Contracts.Crud.Application
{
	/// <summary>
	///   Describes the data for updating an application.
	/// </summary>
	public class UpdateApplicationEntry : BaseApplicationEntry
	{
		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}