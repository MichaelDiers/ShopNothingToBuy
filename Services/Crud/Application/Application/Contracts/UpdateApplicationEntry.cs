namespace Application.Contracts
{
	using Service.Contracts.Crud.Application;
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes the data for updating an application.
	/// </summary>
	public class UpdateApplicationEntry : BaseApplicationEntry, IUpdateApplicationEntry, IEntry<string>
	{
		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}