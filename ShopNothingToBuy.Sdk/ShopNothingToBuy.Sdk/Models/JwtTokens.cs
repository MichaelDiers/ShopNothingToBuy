namespace ShopNothingToBuy.Sdk.Models
{
	using ShopNothingToBuy.Sdk.Contracts;

	public class JwtTokens : IJwtTokens
	{
		public string RefreshToken { get; set; }
		public string Token { get; set; }
	}
}