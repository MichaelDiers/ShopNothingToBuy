namespace User.Services.Models
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes the user data for the update operation of the <see cref="UserService" />.
	/// </summary>
	public class UpdateUserEntry : BaseUser, IEntry<string>
	{
	}
}