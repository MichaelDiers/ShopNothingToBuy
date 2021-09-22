namespace Service.Contracts.Crud.Application
{
	/// <summary>
	///   Describes the data for updating an application.
	/// </summary>
	public interface IUpdateApplicationEntry : IBaseApplicationEntry
	{
		/// <summary>
		///   Gets the original requested id at creation time.
		/// </summary>
		string OriginalId { get; }
	}
}