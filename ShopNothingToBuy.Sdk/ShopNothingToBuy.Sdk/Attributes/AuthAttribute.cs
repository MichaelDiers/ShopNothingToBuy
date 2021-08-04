namespace ShopNothingToBuy.Sdk.Attributes
{
	using System;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using Microsoft.Extensions.DependencyInjection;
	using ShopNothingToBuy.Sdk.Contracts;

	public class AuthAttribute : Attribute, IAsyncActionFilter
	{
		public const string ClaimsTypeToken = nameof(ClaimsTypeToken);
		private const string AuthorizationHeader = "Authorization";
		private readonly Predicate<Claim> claimApplictionRolePredicate;
		private readonly Predicate<Claim> claimTokenTypePredicate;

		public AuthAttribute(bool acceptRefreshToken, string application, params string[] allowedRoles)
		{
			if (string.IsNullOrWhiteSpace(application))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(application));
			}

			if (allowedRoles == null || allowedRoles.Length == 0 || allowedRoles.Any(string.IsNullOrWhiteSpace))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(allowedRoles));
			}

			this.claimApplictionRolePredicate =
				claim => claim.Type == application && allowedRoles.Any(role => claim.Value == role);
			this.claimTokenTypePredicate = claim =>
				claim.Type == IJwtService.ClaimTypeIsRefreshToken
				&& bool.TryParse(claim.Value, out var isRefreshToken)
				&& isRefreshToken == acceptRefreshToken;
		}

		public AuthAttribute(string application, params string[] allowedRoles)
			: this(false, application, allowedRoles)
		{
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.HttpContext.User?.HasClaim(this.claimApplictionRolePredicate) == true
			    && context.HttpContext.User?.HasClaim(this.claimTokenTypePredicate) == true)
			{
				await next();
			}
			else if (context.HttpContext.Request.Headers.TryGetValue(AuthorizationHeader, out var authorizationHeader)
			         && authorizationHeader.Count == 1)
			{
				var bearer = authorizationHeader[0].Split(' ');
				if (bearer.Length == 2 && !string.IsNullOrWhiteSpace(bearer[1]))
				{
					var token = bearer[1];
					var claimsPrincipal = context.HttpContext.RequestServices.GetRequiredService<IJwtService>()
						.ValidateToken(token);

					if (claimsPrincipal != null
					    && claimsPrincipal.HasClaim(this.claimApplictionRolePredicate)
					    && claimsPrincipal.HasClaim(this.claimTokenTypePredicate))
					{
						context.HttpContext.User = claimsPrincipal;
						claimsPrincipal.AddIdentity(
							new ClaimsIdentity(
								new[]
								{
									new Claim(ClaimsTypeToken, token)
								}));

						await next();
					}
				}
			}

			context.Result = new UnauthorizedResult();
		}
	}
}