namespace Service.Contracts.Crud.Application
{
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes the base data of the application.
	/// </summary>
	public interface IBaseApplicationEntry : IEntry<string>
	{
		/// <summary>
		///   Gets the available roles for the application.
		/// </summary>
		public Roles Roles { get; }
	}
}