namespace Service.Sdk.Contracts
{
	/// <summary>
	///   The result of a delete operation.
	/// </summary>
	/// <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Delete" />
	public enum DeleteResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   Entry is deleted.
		/// </summary>
		Deleted = 1,

		/// <summary>
		///   Entry is not found.
		/// </summary>
		NotFound = 2,

		/// <summary>
		///   Given data is invalid.
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