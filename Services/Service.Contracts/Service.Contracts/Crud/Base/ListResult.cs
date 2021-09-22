namespace Service.Contracts.Crud.Base
{
	/// <summary>
	///   The result of a list operation.
	/// </summary>
	/// <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.List" />
	public enum ListResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   List is complete.
		/// </summary>
		Completed = 1,

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