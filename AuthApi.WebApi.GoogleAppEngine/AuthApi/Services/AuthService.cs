namespace AuthApi.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using AuthApi.Contracts;
	using AuthApi.Models;
	using BCrypt.Net;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;
	using ShopNothingToBuy.Sdk.Attributes;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Models;

	/// <summary>
	///   Describes operations in authentication context.
	/// </summary>
	public class AuthService : IAuthService
	{
		/// <summary>
		///   Service for operations on accounts.
		/// </summary>
		private readonly IAccountService accountService;

		/// <summary>
		///   Service for database operations used in authorization context.
		/// </summary>
		private readonly IAuthDatabaseService auhAuthDatabaseService;

		/// <summary>
		///   Service for generating and validating tokens.
		/// </summary>
		private readonly IJwtService jwtService;

		/// <summary>
		///   If a refresh token is used for generating a new token and a new refresh token, the used refresh token is invalidated.
		///   The <see cref="maxRefreshCount" /> specifies how often a new token pair can be generated.
		/// </summary>
		/// <seealso cref="RefreshToken" />
		private readonly int maxRefreshCount;

		/// <summary>
		///   Creates a new instance of <see cref="AuthService" />.
		/// </summary>
		/// <param name="accountService">Service for operations on accounts.</param>
		/// <param name="jwtService">Service for generating and validating tokens.</param>
		/// <param name="auhAuthDatabaseService">Service for database operations used in authorization context.</param>
		/// <param name="configuration">The configuration of the application.</param>
		public AuthService(
			IAccountService accountService,
			IJwtService jwtService,
			IAuthDatabaseService auhAuthDatabaseService,
			IConfiguration configuration,
			ILogger<AuthService> logger)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
			this.auhAuthDatabaseService =
				auhAuthDatabaseService ?? throw new ArgumentNullException(nameof(auhAuthDatabaseService));
			this.maxRefreshCount = configuration.GetValue<int>("Jwt:MaxRefreshCount");
			var c = new JwtConfiguration();
			configuration.Bind("Jwt", c);

			logger.LogError(c.Keys.First().PrivateKey);
			logger.LogError(c.Keys.First().PublicKey);
		}

		/// <summary>
		///   Authenticate the given account.
		/// </summary>
		/// <param name="authentication">The user that is authenticated.</param>
		/// <returns>An <see cref="IJwtTokens" /> containing token and refresh token if authentication succeeds, null otherwise.</returns>
		public async Task<IJwtTokens> Authenticate(Authentication authentication)
		{
			// read account from database
			var account = await this.accountService.Read(authentication.Name);

			// check account exists and password is correct
			if (account is null || account.IsLocked || !BCrypt.Verify(authentication.Password, account.Password))
			{
				return null;
			}

			// create tokens
			var claims = new List<Claim>(account.Claims);
			if (claims.All(claim => claim.Type != IAuthService.ClaimTypeAccountName))
			{
				claims.Add(new Claim(IAuthService.ClaimTypeAccountName, account.Name));
			}

			var tokens = this.jwtService.CreateEncodedJwt(claims);
			if (tokens != null && !string.IsNullOrWhiteSpace(tokens.RefreshToken))
			{
				await this.auhAuthDatabaseService.AddRefreshToken(tokens.RefreshToken);
			}

			return tokens;
		}

		/// <summary>
		///   Create a new token and refresh token.
		/// </summary>
		/// <param name="principal">The <see cref="ClaimsPrincipal" /> that requested the token generation.</param>
		/// <returns>An <see cref="IJwtTokens" /> if the user is granted access to the application and null otherwise.</returns>
		public async Task<IJwtTokens> RefreshToken(ClaimsPrincipal principal)
		{
			if (principal == null)
			{
				return null;
			}

			var accountName = principal.Claims.FirstOrDefault(claim => claim.Type == IAuthService.ClaimTypeAccountName)
				?.Value;
			if (string.IsNullOrWhiteSpace(accountName))
			{
				return null;
			}

			var account = await this.accountService.Read(accountName);
			if (account == null || account.IsLocked)
			{
				return null;
			}

			var token = principal.Claims.FirstOrDefault(claim => claim.Type == AuthAttribute.ClaimsTypeToken)?.Value
				?.ToString();
			if (string.IsNullOrWhiteSpace(token))
			{
				return null;
			}

			var claims = new List<Claim>(account.Claims);
			if (claims.All(claim => claim.Type != IAuthService.ClaimTypeAccountName))
			{
				claims.Add(new Claim(IAuthService.ClaimTypeAccountName, account.Name));
			}

			var newTokens = this.jwtService.CreateEncodedJwt(claims);

			var refreshTokenCounter = await this.auhAuthDatabaseService.ReplaceRefreshToken(token, newTokens.RefreshToken);
			if (refreshTokenCounter < 0 || refreshTokenCounter > this.maxRefreshCount)
			{
				return null;
			}

			return newTokens;
		}
	}
}