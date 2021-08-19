namespace ShopNothingToBuy.Sdk.Extensions
{
	using System;
	using Newtonsoft.Json;
	using ShopNothingToBuy.Sdk.Contracts;

	/// <summary>
	///   Converter for <see cref="LogLevel" />.
	/// </summary>
	public class LogLevelConverter : JsonConverter
	{
		/// <summary>
		///   Checks if <paramref name="objectType" /> can be converted.
		/// </summary>
		/// <param name="objectType">The type of the object to be converted.</param>
		/// <returns>True if an object can be converted and false otherwise.</returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(string);
		}

		/// <summary>
		///   Convert a value to a <see cref="LogLevel" />.
		/// </summary>
		/// <param name="reader">A <see cref="JsonReader" />.</param>
		/// <param name="objectType">The type of the object.</param>
		/// <param name="existingValue">The existing value.</param>
		/// <param name="serializer">The serializer.</param>
		/// <returns>A <see cref="LogLevel" /> as object.</returns>
		public override object ReadJson(
			JsonReader reader,
			Type objectType,
			object existingValue,
			JsonSerializer serializer)
		{
			var value = reader.Value as string;
			return Enum.TryParse<LogLevel>(value, true, out var logLevel) && Enum.IsDefined(typeof(LogLevel), logLevel)
				? logLevel
				: LogLevel.None;
		}

		/// <summary>
		///   Convert a <see cref="LogLevel" /> to json.
		/// </summary>
		/// <param name="writer">A <see cref="JsonWriter" />.</param>
		/// <param name="value">The value to be converted.</param>
		/// <param name="serializer">The serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var logLevel = (LogLevel) value;
			writer.WriteValue(Enum.GetName(typeof(LogLevel), logLevel));
		}
	}
}