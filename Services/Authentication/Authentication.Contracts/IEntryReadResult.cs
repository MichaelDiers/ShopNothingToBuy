namespace Authentication.Contracts
{
	public interface IEntryReadResult<TEntry, TEntryId> where TEntry : class, IEntry<TEntryId>
	{
		ReadResult ReadResult { get; }

		TEntry Result { get; }
	}
}