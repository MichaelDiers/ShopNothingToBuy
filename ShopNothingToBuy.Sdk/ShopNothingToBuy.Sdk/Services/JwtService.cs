namespace ShopNothingToBuy.Sdk.Services
{
	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Security.Claims;
	using System.Security.Cryptography;
	using Microsoft.Extensions.Configuration;
	using Microsoft.IdentityModel.Tokens;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Models;

	public class JwtService : IJwtService
	{
		private const string ClaimTypePublicKeyVersion = "PublicKeyVersion";

		private readonly JwtConfiguration jwtConfiguration = new JwtConfiguration();

		public JwtService(IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			configuration.Bind("Jwt", this.jwtConfiguration);
			if (!this.jwtConfiguration.IsValid())
			{
				throw new ArgumentException("Invalid Jwt configuration.");
			}
		}

		public IJwtTokens CreateEncodedJwt(IEnumerable<Claim> claims)
		{
			var privateKey = this.jwtConfiguration.PrivateKeyAsByteArray();
			using var rsa = RSA.Create();
			rsa.ImportRSAPrivateKey(privateKey, out var _);
			var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
			{
				CryptoProviderFactory = new CryptoProviderFactory
				{
					CacheSignatureProviders = false
				}
			};

			var jwtClaims = new List<Claim>(claims)
			{
				new Claim(ClaimTypePublicKeyVersion, this.jwtConfiguration.KeyVersion.ToString())
			};

			var tokenClaims = new List<Claim>(jwtClaims)
			{
				new Claim(IJwtService.ClaimTypeIsRefreshToken, false.ToString())
			};

			var now = DateTime.UtcNow;

			var token = new JwtSecurityTokenHandler().CreateEncodedJwt(
				this.jwtConfiguration.Issuer,
				this.jwtConfiguration.Audience,
				new ClaimsIdentity(tokenClaims),
				null,
				now.AddMinutes(this.jwtConfiguration.ExpiresToken),
				now,
				signingCredentials);

			jwtClaims.Add(new Claim(IJwtService.ClaimTypeIsRefreshToken, true.ToString()));
			var refreshToken = new JwtSecurityTokenHandler().CreateEncodedJwt(
				this.jwtConfiguration.Issuer,
				this.jwtConfiguration.Audience,
				new ClaimsIdentity(jwtClaims),
				null,
				now.AddMinutes(this.jwtConfiguration.ExpiresRefresh),
				now,
				signingCredentials);

			return new JwtTokens
			{
				Token = token,
				RefreshToken = refreshToken
			};
		}

		public ClaimsPrincipal ValidateToken(string token)
		{
			try
			{
				// read version of key
				var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
				if (!int.TryParse(
					jwt?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypePublicKeyVersion)?.Value?.ToString(),
					out var publicKeyVersion))
				{
					return null;
				}

				// read public key
				var publicKey = this.jwtConfiguration.PublicKeyAsByteArray(publicKeyVersion);

				// token check
				using var rsa = RSA.Create();
				rsa.ImportSubjectPublicKeyInfo(publicKey, out var _);
				var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(
					token,
					new TokenValidationParameters
					{
						IssuerSigningKey = new RsaSecurityKey(rsa),
						ValidAudience = this.jwtConfiguration.Audience,
						ValidIssuer = this.jwtConfiguration.Issuer,
						RequireAudience = true,
						RequireExpirationTime = true,
						RequireSignedTokens = true,
						ValidateIssuer = true,
						CryptoProviderFactory = new CryptoProviderFactory
						{
							CacheSignatureProviders = false
						}
					},
					out var _);

				return claimsPrincipal;
			}
			catch
			{
				// token is invalid
				return null;
			}
		}
	}
}