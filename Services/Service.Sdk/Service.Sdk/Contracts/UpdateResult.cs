namespace Service.Sdk.Contracts
{
	/// <summary>
	///   The result of an update operation.
	/// </summary>
	/// <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Update" />
	public enum UpdateResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   The entry is updated.
		/// </summary>
		Updated = 1,

		/// <summary>
		///   The entry is not found.
		/// </summary>
		NotFound = 2,

		/// <summary>
		///   The input data is not valid.
		/// </summary>
		InvalidData = 3,

		/// <summary>
		///   An unexpected error occurred.
		/// </summary>
		InternalError = 4
	}
}