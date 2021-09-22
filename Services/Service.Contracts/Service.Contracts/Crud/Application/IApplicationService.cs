namespace Service.Contracts.Crud.Application
{
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes operations on <see cref="IApplicationEntry" /> instances.
	/// </summary>
	public interface
		IApplicationService : IServiceBase<IApplicationEntry, string, ICreateApplicationEntry, IUpdateApplicationEntry>
	{
	}
}