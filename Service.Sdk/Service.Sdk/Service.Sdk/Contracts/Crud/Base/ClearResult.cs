namespace Service.Sdk.Contracts.Crud.Base
{
	/// <summary>
	///   The result of a clear operation.
	/// </summary>
	/// <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Clear" />
	public enum ClearResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   All known entries are deleted.
		/// </summary>
		Cleared = 1,

		/// <summary>
		///   An unexpected error occurred.
		/// </summary>
		InternalError = 2,

		/// <summary>
		///   The operation is not allowed.
		/// </summary>
		Unauthorized = 3
	}
}