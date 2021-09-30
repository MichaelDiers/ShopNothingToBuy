namespace Service.Sdk.Contracts.Crud.User
{
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Describes the service for user processing.
	/// </summary>
	public interface IUserService : IServiceBase<UserEntry, string, CreateUserEntry, UpdateUserEntry>
	{
	}
}