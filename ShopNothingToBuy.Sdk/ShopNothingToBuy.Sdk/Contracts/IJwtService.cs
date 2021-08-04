namespace ShopNothingToBuy.Sdk.Contracts
{
	using System.Collections.Generic;
	using System.Security.Claims;

	public interface IJwtService
	{
		public const string ClaimTypeIsRefreshToken = "IsRefreshToken";

		IJwtTokens CreateEncodedJwt(IEnumerable<Claim> claims);

		ClaimsPrincipal ValidateToken(string token);
	}
}