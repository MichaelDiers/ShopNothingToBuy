namespace Authentication.Contracts
{
	public interface IServiceBase<in TEntry, in TEntryId> where TEntry : class, IEntry<TEntryId>
	{
		CreateResult Create(TEntry entry);

		DeleteResult Delete(TEntryId id);

		ReadResult Read(TEntryId id);

		UpdateResult Update(TEntry entry);
	}
}