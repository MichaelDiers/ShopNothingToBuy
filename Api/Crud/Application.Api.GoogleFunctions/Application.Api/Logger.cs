namespace Application.Api
{
	using System;
	using System.Threading.Tasks;
	using Service.Contracts.Business.Log;

	/// <summary>
	///   Todo replace with service.
	/// </summary>
	public class Logger : ILogger
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