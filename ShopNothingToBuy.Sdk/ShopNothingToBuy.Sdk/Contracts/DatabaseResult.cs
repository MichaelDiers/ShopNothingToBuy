namespace ShopNothingToBuy.Sdk.Contracts
{
	using System;

	/// <summary>
	///   Specifies the result of a database operation.
	/// </summary>
	[Flags]
	public enum DatabaseResult
	{
		/// <summary>
		///   Undefined result.
		/// </summary>
		None = 0,

		/// <summary>
		///   Entry already exists in the database.
		/// </summary>
		AlreadyExists = 1 << 1,

		/// <summary>
		///   Database error occurred.
		/// </summary>
		Error = 1 << 2,

		/// <summary>
		///   New database entry is created.
		/// </summary>
		Created = 1 << 3,

		/// <summary>
		///   Database entry is deleted.
		/// </summary>
		Deleted = 1 << 4,

		/// <summary>
		///   Database entry does not exist in the database.
		/// </summary>
		NotFound = 1 << 5,

		/// <summary>
		///   Database entry is updated.
		/// </summary>
		Updated = 1 << 6
	}
}