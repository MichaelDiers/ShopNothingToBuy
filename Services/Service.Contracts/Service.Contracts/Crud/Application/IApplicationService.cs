namespace Service.Contracts.Crud.Application
{
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes operations on <see cref="ApplicationEntry" /> instances.
	/// </summary>
	public interface
		IApplicationService : IServiceBase<ApplicationEntry, string, CreateApplicationEntry, UpdateApplicationEntry>
	{
	}
}