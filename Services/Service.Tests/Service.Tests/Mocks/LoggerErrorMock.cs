namespace Service.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Contracts.Business.Log;

	/// <summary>
	///   An error logger for messages and <see cref="Exception" />s. The mock does not process the input but throws a
	///   <see cref="NotImplementedException" />.
	/// </summary>
	public class LoggerErrorMock : ILogger
	{
		/// <summary>
		///   No input processing.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public Task Error(string message)
		{
			throw new NotImplementedException(nameof(LoggerErrorMock));
		}

		/// <summary>
		///   No input processing.
		/// </summary>
		/// <param name="message">The error message to be logged.</param>
		/// <param name="ex">The <see cref="Exception" /> to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public Task Error(string message, Exception ex)
		{
			throw new NotImplementedException(nameof(LoggerErrorMock));
		}
	}
}