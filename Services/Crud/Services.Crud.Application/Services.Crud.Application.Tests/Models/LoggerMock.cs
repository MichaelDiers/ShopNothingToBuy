namespace Services.Crud.Application.Tests.Models
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Business.Log;

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