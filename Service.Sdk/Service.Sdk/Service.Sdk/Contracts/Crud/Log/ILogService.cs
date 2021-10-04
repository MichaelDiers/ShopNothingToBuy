namespace Service.Sdk.Contracts.Crud.Log
{
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Describes the operations of the log service.
	/// </summary>
	public interface ILogService : IServiceBase<LogEntry, string, CreateLogEntry, UpdateLogEntry>
	{
	}
}