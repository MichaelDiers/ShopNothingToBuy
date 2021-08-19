namespace ShopNothingToBuy.Sdk.Models
{
	using Newtonsoft.Json;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Extensions;

	/// <summary>
	///   Data-transfer object for log entries.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class LogEntryDto
	{
		/// <summary>
		///   Gets or sets the <see cref="LogLevel" /> of the entry.
		/// </summary>
		[JsonProperty("level", Required = Required.DisallowNull)]
		[System.Text.Json.Serialization.JsonConverter(typeof(LogLevelConverter))]
		public LogLevel Level { get; set; }

		/// <summary>
		///   Gets or sets the log message.
		/// </summary>
		[JsonProperty("message", Required = Required.DisallowNull)]
		public string Message { get; set; }
	}
}