namespace Service.Sdk.Contracts.Crud.Log
{
	/// <summary>
	///   The log level context.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		///   Undefined value.
		/// </summary>
		None = 0,

		/// <summary>
		///   Log entry created in debug context.
		/// </summary>
		Debug = 1,

		/// <summary>
		///   Log entry is only an info.
		/// </summary>
		Info = 2,

		/// <summary>
		///   A log entry that is logged in error context.
		/// </summary>
		Error = 3
	}
}