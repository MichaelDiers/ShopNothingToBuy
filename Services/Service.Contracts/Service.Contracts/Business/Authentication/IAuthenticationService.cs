namespace Service.Contracts.Business.Authentication
{
	using System.Threading.Tasks;
	using Service.Contracts.Base;

	/// <summary>
	///   Describes the operations of the authentication service.
	/// </summary>
	public interface IAuthenticationService
	{
		/// <summary>
		///   Authenticates a user and creates a new jwt.
		/// </summary>
		/// <param name="request">The request data.</param>
		/// <returns>
		///   A Task whose result is an <see cref="IServiceResponse{TOperationResult,TResponse}" /> including the result and
		///   a jwt if the operation succeeds.
		/// </returns>
		Task<IServiceResponse<AuthenticationResult, string>> Authenticate(IAuthenticationRequest request);

		/// <summary>
		///   Register a new user.
		/// </summary>
		/// <param name="request">The request data.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IServiceResponse{TOperationResult,TResponse}" /> including
		///   the <see cref="RegisterResult" /> and a jwt if the operations succeeds.
		/// </returns>
		Task<IServiceResponse<RegisterResult, string>> Register(IRegisterRequest request);
	}
}