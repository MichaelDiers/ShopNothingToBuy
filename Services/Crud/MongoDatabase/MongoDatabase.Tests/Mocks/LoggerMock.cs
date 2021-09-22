namespace MongoDatabase.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;

	internal class LoggerMock : ILogger
	{
		public Task Error(string message)
		{
			return Task.CompletedTask;
		}

		public Task Error(string message, Exception ex)
		{
			return Task.CompletedTask;
		}
	}
}