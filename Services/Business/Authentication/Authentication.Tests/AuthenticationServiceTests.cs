namespace Service.Business.Authentication.Tests
{
	using System.Threading.Tasks;
	using Service.Contracts.Business.Authentication;
	using Service.Models.Business.Authentication;
	using Xunit;

	public class AuthenticationServiceTests
	{
		[Theory]
		[InlineData("username", "password1", AuthenticationResult.Authorized)]
		[InlineData("username", "password2", AuthenticationResult.Failed)]
		public async void Authenticate(string userName, string password, AuthenticationResult expectedResult)
		{
			await CallAuthenticate(
				new AuthenticationService(),
				userName,
				password,
				expectedResult);
		}

		[Theory]
		[InlineData("username1", "password1", RegisterResult.Registered)]
		[InlineData("username2", "password2", RegisterResult.Registered)]
		[InlineData(null, "password3", RegisterResult.UserInvalid)]
		[InlineData("", "password3", RegisterResult.UserInvalid)]
		[InlineData("username3", null, RegisterResult.PasswordInvalid)]
		[InlineData("username3", "", RegisterResult.PasswordInvalid)]
		public async void Register(string userName, string password, RegisterResult expectedResult)
		{
			await CallRegister(
				new AuthenticationService(),
				userName,
				password,
				expectedResult);
		}

		[Theory]
		[InlineData(
			"username_exists",
			"password1",
			RegisterResult.Registered,
			RegisterResult.UserExists)]
		public async void Register_ForExistingUser(
			string userName,
			string password,
			RegisterResult expectedResultRegister,
			RegisterResult expectedResultReRegister)
		{
			var service = new AuthenticationService();
			await CallRegister(
				service,
				userName,
				password,
				expectedResultRegister);
			await CallRegister(
				service,
				userName,
				password,
				expectedResultReRegister);
		}

		private static async Task CallAuthenticate(
			IAuthenticationService service,
			string userName,
			string password,
			AuthenticationResult expectedResult)
		{
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

		private static async Task CallRegister(
			IAuthenticationService service,
			string userName,
			string password,
			RegisterResult expectedResult)
		{
			var actual = await service.Register(
				new RegisterRequest
				{
					Password = password,
					UserName = userName
				});

			Assert.Equal(expectedResult, actual.Result);
			if (expectedResult == RegisterResult.Registered)
			{
				Assert.NotNull(actual.Response);
			}
			else
			{
				Assert.Null(actual.Response);
			}
		}
	}
}