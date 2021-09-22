namespace Service.Contracts.Crud.Application
{
	/// <summary>
	///   Describes an application.
	/// </summary>
	public interface IApplicationEntry : IBaseApplicationEntry
	{
		/// <summary>
		///   Gets the original requested id at creation time.
		/// </summary>
		string OriginalId { get; }
	}
}