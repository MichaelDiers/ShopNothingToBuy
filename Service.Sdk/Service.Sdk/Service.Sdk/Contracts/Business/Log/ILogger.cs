namespace Service.Sdk.Contracts.Business.Log
{
	using System;
	using System.Threading.Tasks;

	/// <summary>
	///   An error logger for messages and <see cref="Exception" />s.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		///   Logs an error message.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		Task Error(string message);

		/// <summary>
		///   Logs an error message including an <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The error message to be logged.</param>
		/// <param name="ex">The <see cref="Exception" /> to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		Task Error(string message, Exception ex);
	}
}