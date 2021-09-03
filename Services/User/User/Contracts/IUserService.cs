namespace User.Contracts
{
	using Service.Sdk.Contracts;
	using User.Services.Models;

	/// <summary>
	///   Describes the service for user processing.
	/// </summary>
	public interface IUserService : IServiceBase<UserEntry, string, CreateUserEntry, UpdateUserEntry>
	{
	}
}