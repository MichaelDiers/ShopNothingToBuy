namespace LogApi
{
	using System;
	using MongoDB.Bson;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Models;

	/// <summary>
	///   Specification of a log entry.
	/// </summary>
	public class LogEntry : IDatabaseEntry<ObjectId>
	{
		/// <summary>
		///   Creates a new instance of <see cref="LogEntry" />.
		/// </summary>
		public LogEntry()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="LogEntry" />.
		/// </summary>
		/// <param name="logEntryDto">Specifies the values of the log entry.</param>
		public LogEntry(LogEntryDto logEntryDto)
		{
			this.Level = logEntryDto.Level;
			this.Message = logEntryDto.Message;
			this.Id = ObjectId.GenerateNewId(DateTime.UtcNow);
		}

		/// <summary>
		///   Gets or sets the <see cref="LogLevel" /> of the entry.
		/// </summary>
		public LogLevel Level { get; set; }

		/// <summary>
		///   Gets or sets the log message.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		///   Gets or sets the id of the entry.
		/// </summary>
		public ObjectId Id { get; set; }
	}
}