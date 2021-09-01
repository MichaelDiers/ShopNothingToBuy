namespace Service.Sdk.Contracts
{
	/// <summary>
	///   Specifies an entry with an id.
	/// </summary>
	/// <typeparam name="TEntryId">The type of the id.</typeparam>
	public interface IEntry<out TEntryId>
	{
		/// <summary>
		///   Gets the id of the entry.
		/// </summary>
		TEntryId Id { get; }
	}
}