namespace AuthApi.Contracts
{
	using System.Security.Claims;
	using System.Threading.Tasks;
	using AuthApi.Models;
	using ShopNothingToBuy.Sdk.Contracts;

	/// <summary>
	///   Describes operations in authentication context.
	/// </summary>
	public interface IAuthService
	{
		/// <summary>
		///   The type used for the account name.
		/// </summary>
		public const string ClaimTypeAccountName = "AccountName";

		/// <summary>
		///   Authenticate the user by its account data and create token and refresh token.
		/// </summary>
		/// <param name="authentication">The authentication data of the account.</param>
		/// <returns><see cref="IJwtTokens" /> or null if authentication failed.</returns>
		Task<IJwtTokens> Authenticate(Authentication authentication);

		/// <summary>
		///   Create a new token and refresh token pair for the given principal.
		/// </summary>
		/// <param name="principal">The principal that requested a new token pair.</param>
		/// <returns><see cref="IJwtTokens" /> or null if authentication failed.</returns>
		Task<IJwtTokens> RefreshToken(ClaimsPrincipal principal);
	}
}