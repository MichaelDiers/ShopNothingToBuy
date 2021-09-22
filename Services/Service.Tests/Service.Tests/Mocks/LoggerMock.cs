namespace Service.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Contracts.Business.Log;

	/// <summary>
	///   An error logger for messages and <see cref="Exception" />s. The mock does not process the input.
	/// </summary>
	public class LoggerMock : ILogger
	{
		/// <summary>
		///   Gets a value that is equal to the number of <see cref="ILogger" /> method calls.
		/// </summary>
		public int CallCount { get; private set; }

		/// <summary>
		///   No input processing.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public virtual Task Error(string message)
		{
			++this.CallCount;
			return Task.CompletedTask;
		}

		/// <summary>
		///   No input processing.
		/// </summary>
		/// <param name="message">The error message to be logged.</param>
		/// <param name="ex">The <see cref="Exception" /> to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public virtual Task Error(string message, Exception ex)
		{
			++this.CallCount;
			return Task.CompletedTask;
		}
	}
}