namespace Service.Sdk.Contracts.Crud.Log
{
	using System;

	/// <summary>
	///   Base data of a log entry.
	/// </summary>
	public class BaseLogEntry
	{
		/// <summary>
		///   Gets or sets the id of the application that logged the messages.
		/// </summary>
		public string ApplicationId { get; set; }

		/// <summary>
		///   Gets or sets the exception that is raised.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		///   Gets or sets the log level.
		/// </summary>
		public LogLevel LogLevel { get; set; }

		/// <summary>
		///   Gets or sets the log message.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		///   Gets or sets the id of the process in whose context the message is logged.
		/// </summary>
		public string ProcessId { get; set; }
	}
}