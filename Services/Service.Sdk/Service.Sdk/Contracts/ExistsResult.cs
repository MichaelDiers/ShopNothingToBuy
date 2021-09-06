namespace Service.Sdk.Contracts
{
	/// <summary>
	///   The result of a exists operation.
	/// </summary>
	/// <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Exists" />
	public enum ExistsResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   Entry exists.
		/// </summary>
		Exists = 1,

		/// <summary>
		///   Entry does not exist.
		/// </summary>
		NotFound = 2,

		/// <summary>
		///   Invalid input data.
		/// </summary>
		InvalidData = 3,

		/// <summary>
		///   An unexpected error occurred.
		/// </summary>
		InternalError = 4,

		/// <summary>
		///   The operation is not allowed.
		/// </summary>
		Unauthorized = 5
	}
}