namespace Service.Models.Business.Authentication
{
	using Service.Contracts.Business.Authentication;

	/// <summary>
	///   Describes a registration request.
	/// </summary>
	public class RegisterRequest : AuthenticationBase, IRegisterRequest
	{
	}
}