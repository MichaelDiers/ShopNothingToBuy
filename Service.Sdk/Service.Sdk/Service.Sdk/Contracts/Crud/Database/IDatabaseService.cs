namespace Service.Sdk.Contracts.Crud.Database
{
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Describes a database service.
	/// </summary>
	/// <typeparam name="TEntry">The type of the database entries.</typeparam>
	/// <typeparam name="TEntryId">The type of the database entry id.</typeparam>
	public interface IDatabaseService<TEntry, TEntryId> : IServiceBase<TEntry, TEntryId, TEntry, TEntry>
		where TEntry : class, IEntry<TEntryId>
	{
	}
}