namespace ShopNothingToBuy.Sdk.Contracts
{
	public interface IJwtTokens
	{
		string RefreshToken { get; }
		string Token { get; }
	}
}