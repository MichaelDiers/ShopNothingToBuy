namespace Service.Sdk.Contracts
{
	/// <summary>
	///   The result of a create operation.
	/// </summary>
	/// <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Create" />
	public enum CreateResult
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   An entry is created.
		/// </summary>
		Created = 1,

		/// <summary>
		///   The entry already exists.
		/// </summary>
		AlreadyExists = 2,

		/// <summary>
		///   The input data are not valid.
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