namespace Service.Contracts.Crud.Application
{
	/// <summary>
	///   Describes the data for creating an application.
	/// </summary>
	public interface ICreateApplicationEntry
	{
		/// <summary>
		///   Gets the id of the application.
		/// </summary>
		string Id { get; }

		/// <summary>
		///   Gets the available roles for the application.
		/// </summary>
		Roles Roles { get; }
	}
}