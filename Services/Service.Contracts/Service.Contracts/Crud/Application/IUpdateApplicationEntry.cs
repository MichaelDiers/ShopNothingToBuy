namespace Service.Contracts.Crud.Application
{
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes the data for updating an application.
	/// </summary>
	public interface IUpdateApplicationEntry : IEntry<string>
	{
		/// <summary>
		///   Gets the original requested id at creation time.
		/// </summary>
		string OriginalId { get; }

		/// <summary>
		///   Gets the available roles for the application.
		/// </summary>
		Roles Roles { get; }
	}
}