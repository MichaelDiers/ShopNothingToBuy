namespace Service.Business.Authentication
{
	using System.Threading.Tasks;
	using Service.Contracts.Base;
	using Service.Contracts.Business.Authentication;
	using Service.Models.Base;

	/// <summary>
	///   Describes the operations of the authentication service.
	/// </summary>
	public class AuthenticationService : IAuthenticationService
	{
		/// <summary>
		///   Authenticates a user and creates a new jwt.
		/// </summary>
		/// <param name="request">The request data.</param>
		/// <returns>
		///   A Task whose result is an <see cref="IServiceResponse{TOperationResult,TResponse}" /> including the result and
		///   a jwt if the operation succeeds.
		/// </returns>
		public Task<IServiceResponse<AuthenticationResult, string>> Authenticate(IAuthenticationRequest request)
		{
			var serviceResponse = new ServiceResponse<AuthenticationResult, string>
			{
				Result = AuthenticationResult.Authorized,
				Response = string.Empty
			};

			var response = (IServiceResponse<AuthenticationResult, string>) serviceResponse;
			return Task.FromResult(response);
		}

		/// <summary>
		///   Register a new user.
		/// </summary>
		/// <param name="request">The request data.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IServiceResponse{TOperationResult,TResponse}" /> including
		///   the <see cref="RegisterResult" /> and a jwt if the operations succeeds.
		/// </returns>
		public Task<IServiceResponse<RegisterResult, string>> Register(IRegisterRequest request)
		{
			var serviceResponse = new ServiceResponse<RegisterResult, string>
			{
				Result = RegisterResult.Registered,
				Response = string.Empty
			};

			var response = (IServiceResponse<RegisterResult, string>) serviceResponse;
			return Task.FromResult(response);
		}
	}
}