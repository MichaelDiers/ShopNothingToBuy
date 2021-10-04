namespace Service.Sdk.Contracts.Crud.Log
{
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Describes a log entry.
	/// </summary>
	public class LogEntry : BaseLogEntry, IEntry<string>
	{
		/// <summary>
		///   Gets or sets the id of the log entry.
		/// </summary>
		public string Id { get; set; }
	}
}