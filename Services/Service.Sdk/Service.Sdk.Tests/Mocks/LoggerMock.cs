namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;

	internal class LoggerMock : IExtendedLogger
	{
		public string ErrorMessage { get; private set; }
		public int ErrorMessageAndExceptionCallCount { get; private set; }
		public int ErrorMessageCallCount { get; private set; }
		public Exception Exception { get; private set; }

		public Task Error(string message)
		{
			this.ErrorMessageCallCount += 1;
			this.ErrorMessage = message;
			return Task.CompletedTask;
		}

		public Task Error(string message, Exception ex)
		{
			this.ErrorMessageAndExceptionCallCount += 1;
			this.ErrorMessage = message;
			this.Exception = ex;
			return Task.CompletedTask;
		}
	}
}