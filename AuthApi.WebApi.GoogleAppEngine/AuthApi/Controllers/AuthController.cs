namespace AuthApi.Controllers
{
	using System;
	using System.Threading.Tasks;
	using AuthApi.Attributes;
	using AuthApi.Contracts;
	using AuthApi.Models;
	using Microsoft.AspNetCore.Mvc;
	using ShopNothingToBuy.Sdk.Attributes;

	/// <summary>
	///   Routes for the authentication of known accounts.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiKey]
	public class AuthController : ControllerBase
	{
		/// <summary>
		///   Service for the authentication of accounts.
		/// </summary>
		private readonly IAuthService authService;

		/// <summary>
		///   Creates a new instance of <see cref="AuthController" />.
		/// </summary>
		/// <param name="authService">Service for the authentication of accounts.</param>
		public AuthController(IAuthService authService)
		{
			this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
		}

		/// <summary>
		///   Authenticate an account by its <see cref="Authentication.Name" /> and <see cref="Authentication.Password" />.
		/// </summary>
		/// <param name="authentication">The account information.</param>
		/// <returns>
		///   An <see cref="OkObjectResult" /> including token and refresh token or an <see cref="UnauthorizedResult" />
		///   otherwise.
		/// </returns>
		[HttpPost]
		public async Task<IActionResult> Authenticate([FromBody] Authentication authentication)
		{
			var jwtTokens = await this.authService.Authenticate(authentication);
			return jwtTokens != null ? (IActionResult) new OkObjectResult(jwtTokens) : new UnauthorizedResult();
		}

		/// <summary>
		///   Use a refresh token to generate a new token and refresh token pair. Invalidates the used refresh token.
		/// </summary>
		/// <returns>
		///   An <see cref="OkObjectResult" /> including token and refresh token or an <see cref="UnauthorizedResult" />
		///   otherwise.
		/// </returns>
		[HttpPut]
		[CustomAuth(true, AuthRole.All)]
		public async Task<IActionResult> RefreshToken()
		{
			var tokens = await this.authService.RefreshToken(this.User);
			return tokens != null ? (IActionResult) new OkObjectResult(tokens) : new UnauthorizedResult();
		}
	}
}