namespace Service.Sdk.Contracts
{
	/// <summary>
	///   The result of a read operation.
	/// </summary>
	/// <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Read" />
	public enum ReadResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   The operation did succeed.
		/// </summary>
		Read = 1,

		/// <summary>
		///   The specified entry is not found.
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