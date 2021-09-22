namespace Service.Business.Authentication.Tests
{
	using Service.Contracts.Business.Authentication;
	using Service.Models.Business.Authentication;
	using Xunit;

	public class AuthenticationServiceTests
	{
		[Theory]
		[InlineData("username", "password", AuthenticationResult.Authorized)]
		public async void Authenticate(string userName, string password, AuthenticationResult expectedResult)
		{
			var service = new AuthenticationService();
			var actual = await service.Authenticate(
				new AuthenticationRequest
				{
					Password = password,
					UserName = userName
				});

			Assert.Equal(expectedResult, actual.Result);
			if (expectedResult == AuthenticationResult.Authorized)
			{
				Assert.NotNull(actual.Response);
			}
			else
			{
				Assert.Null(actual.Response);
			}
		}

		[Fact]
		public async void Register()
		{
			var service = new AuthenticationService();
			var actual = await service.Register(new RegisterRequest());

			Assert.Equal(RegisterResult.Registered, actual.Result);
			Assert.NotNull(actual.Response);
		}
	}
}