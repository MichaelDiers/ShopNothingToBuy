namespace AuthApi.Controllers
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using AuthApi.Attributes;
	using AuthApi.Contracts;
	using AuthApi.Models;
	using BCrypt.Net;
	using Microsoft.AspNetCore.Mvc;
	using ShopNothingToBuy.Sdk.Attributes;

	/// <summary>
	///   Defines routes for CRUD operations on accounts.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiKey]
	public class AccountController : ControllerBase
	{
		/// <summary>
		///   Service for account operations.
		/// </summary>
		private readonly IAccountService accountService;

		/// <summary>
		///   Creates a new instance of <see cref="AccountController" />.
		/// </summary>
		/// <param name="accountService">Service for account operations.</param>
		public AccountController(IAccountService accountService)
		{
			this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		/// <summary>
		///   Create a new account.
		/// </summary>
		/// <param name="accountDto">An account is created from the given data.</param>
		/// <returns>
		///   A <see cref="BadRequestResult" /> if the request is invalid. A <see cref="ConflictResult" /> if an account
		///   with <see cref="Authentication.Name" /> already exists. A <see cref="StatusCodeResult" /> with code 500 if an
		///   internal error occurred. A <see cref="CreatedResult" /> if the request is valid, the account does not already exists
		///   and is created.
		/// </returns>
		[HttpPost]
		[CustomAuth(AuthRole.Writer, AuthRole.Admin)]
		public async Task<IActionResult> Create([FromBody] AccountDto accountDto)
		{
			if (string.IsNullOrWhiteSpace(accountDto.Password))
			{
				return new BadRequestResult();
			}

			var newAccount = new Account(accountDto);
			var (account, isConflict, isBadRequest) = await this.accountService.Create(newAccount);
			if (account != null)
			{
				return new CreatedResult(string.Empty, account);
			}

			if (isConflict)
			{
				return new ConflictResult();
			}

			return isBadRequest ? new BadRequestResult() : new StatusCodeResult(500);
		}

		/// <summary>
		///   Delete an account by the given <paramref name="accountName" />.
		/// </summary>
		/// <param name="accountName">The name of the account to be deleted.</param>
		/// <returns>An <see cref="NoContentResult" /> if the account is deleted and <see cref="NotFoundResult" /> otherwise.</returns>
		[HttpDelete]
		[Route("{accountName}")]
		[CustomAuth(AuthRole.Admin)]
		[StringLength("accountName", Authentication.NameMinLength, Authentication.NameMaxLength)]
		public async Task<IActionResult> Delete([FromRoute] string accountName)
		{
			return await this.accountService.Delete(accountName)
				? (IActionResult) new NoContentResult()
				: new NotFoundResult();
		}

		/// <summary>
		///   Delete all accounts of the application.
		/// </summary>
		/// <returns>
		///   A <see cref="NoContentResult" /> if the operation succeeds and a <see cref="StatusCodeResult" /> with code 500
		///   otherwise.
		/// </returns>
		[HttpDelete]
		[Route("all")]
		[CustomAuth(AuthRole.Admin)]
		public async Task<IActionResult> Delete()
		{
			return await this.accountService.Delete() ? new NoContentResult() : new StatusCodeResult(500);
		}

		/// <summary>
		///   Read an account by its name.
		/// </summary>
		/// <param name="accountName">The name of the account.</param>
		/// <returns>An <see cref="OkObjectResult" /> containing account data or if not found a <see cref="NotFoundResult" />.</returns>
		[HttpGet]
		[Route("{accountName}")]
		[CustomAuth(AuthRole.Reader, AuthRole.Writer, AuthRole.Admin)]
		[StringLength("accountName", Authentication.NameMinLength, Authentication.NameMaxLength)]
		public async Task<IActionResult> Read([FromRoute] string accountName)
		{
			var account = await this.accountService.Read(accountName);
			if (account != null)
			{
				return new OkObjectResult(new AccountDto(account));
			}

			return new NotFoundResult();
		}

		/// <summary>
		///   Read all available accounts.
		/// </summary>
		/// <returns>An <see cref="OkObjectResult" /> containing a list of accounts.</returns>
		[HttpGet]
		[Route("all")]
		[CustomAuth(AuthRole.Reader, AuthRole.Writer, AuthRole.Admin)]
		public async Task<IActionResult> Read()
		{
			var accounts = await this.accountService.Read();
			return new OkObjectResult(accounts.Select(account => new AccountDto(account)));
		}

		/// <summary>
		///   Update an account.
		/// </summary>
		/// <param name="accountDto">Replaces the current account data for <see cref="Authentication.Name" />.</param>
		/// <returns>An <see cref="OkResult" /> if update succeeds and a <see cref="NotFoundResult" /> otherwise.</returns>
		[HttpPut]
		[CustomAuth(AuthRole.Writer, AuthRole.Admin)]
		public async Task<IActionResult> Update([FromBody] AccountDto accountDto)
		{
			var updatedAccount = new Account(accountDto);
			if (!string.IsNullOrWhiteSpace(updatedAccount.Password))
			{
				updatedAccount.Password = BCrypt.HashPassword(updatedAccount.Password);
			}

			return await this.accountService.Update(updatedAccount)
				? (IActionResult) new OkResult()
				: new NotFoundResult();
		}
	}
}