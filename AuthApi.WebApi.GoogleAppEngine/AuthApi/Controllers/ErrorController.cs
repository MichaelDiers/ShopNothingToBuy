namespace AuthApi.Controllers
{
	using AuthApi.Attributes;
	using AuthApi.Contracts;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using ShopNothingToBuy.Sdk.Attributes;
	using ShopNothingToBuy.Sdk.Controller;

	/// <summary>
	///   Handles application errors. The routes are defined in <see cref="ErrorControllerBase" />.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiKey]
	[CustomAuth(AuthRole.All)]
	public class ErrorController : ErrorControllerBase
	{
		/// <summary>
		///   Creates a new instance of <see cref="ErrorController" />.
		/// </summary>
		/// <param name="logger">Logger for error messages.</param>
		public ErrorController(ILogger<ErrorControllerBase> logger)
			: base(logger)
		{
		}
	}
}