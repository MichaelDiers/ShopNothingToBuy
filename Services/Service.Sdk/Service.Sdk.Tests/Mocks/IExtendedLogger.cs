namespace Service.Sdk.Tests.Mocks
{
	using System;
	using Service.Contracts.Business.Log;

	public interface IExtendedLogger : ILogger
	{
		string ErrorMessage { get; }
		int ErrorMessageAndExceptionCallCount { get; }
		int ErrorMessageCallCount { get; }

		Exception Exception { get; }
	}
}