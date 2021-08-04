namespace AuthApi.Controllers
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using AuthApi.Attributes;
	using AuthApi.Contracts;
	using AuthApi.Models;
	using Microsoft.AspNetCore.Mvc;
	using ShopNothingToBuy.Sdk.Attributes;

	/// <summary>
	///   Defines routes for CRUD operations on applications.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiKey]
	public class ApplicationController : ControllerBase
	{
		/// <summary>
		///   Service for processing applications.
		/// </summary>
		private readonly IApplicationService applicationService;

		/// <summary>
		///   Creates a new instance of <see cref="ApplicationController" />.
		/// </summary>
		/// <param name="applicationService">Service for processing applications.</param>
		public ApplicationController(IApplicationService applicationService)
		{
			this.applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
		}

		/// <summary>
		///   Create a new application.
		/// </summary>
		/// <param name="createApplicationDto">A data-transfer object for the new application.</param>
		/// <returns>
		///   A <see cref="StatusCodeResult" /> with code 500 if the application is not created and
		///   <see cref="CreatedResult" /> including the <see cref="ApplicationDto" /> otherwise.
		/// </returns>
		[HttpPost]
		[CustomAuth(AuthRole.Writer, AuthRole.Admin)]
		public async Task<IActionResult> Create([FromBody] CreateApplicationDto<CreateRoleDto> createApplicationDto)
		{
			var application = await this.applicationService.Create(new Application(createApplicationDto));
			if (application is null)
			{
				return new StatusCodeResult(500);
			}

			return new CreatedResult(string.Empty, new ApplicationDto(application));
		}

		/// <summary>
		///   Delete an application by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="id">The id of the application.</param>
		/// <returns>A <see cref="NoContentResult" /> if the operation succeeds and <see cref="NotFoundResult" /> otherwise.</returns>
		[HttpDelete]
		[Route("{id}")]
		[CustomAuth(AuthRole.Admin)]
		[ValidateStringIsGuid("id")]
		public async Task<IActionResult> Delete([FromRoute] string id)
		{
			if (await this.applicationService.Delete(id))
			{
				return new NoContentResult();
			}

			return new NotFoundResult();
		}

		/// <summary>
		///   Delete all applications.
		/// </summary>
		/// <returns>
		///   A <see cref="NoContentResult" /> if the operation succeeds and <see cref="StatusCodeResult" /> with code 500
		///   otherwise.
		/// </returns>
		[HttpDelete]
		[Route("all")]
		[CustomAuth(AuthRole.Admin)]
		public async Task<IActionResult> DeleteApplications()
		{
			return await this.applicationService.DeleteApplications() ? new NoContentResult() : new StatusCodeResult(500);
		}

		/// <summary>
		///   Read application data by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="id">The id of the application.</param>
		/// <returns>
		///   An <see cref="OkObjectResult" /> containing the <see cref="ApplicationDto" /> or a
		///   <see cref="NotFoundResult" /> otherwise.
		/// </returns>
		[HttpGet]
		[Route("{id}")]
		[CustomAuth(AuthRole.Reader, AuthRole.Writer, AuthRole.Admin)]
		[ValidateStringIsGuid("id")]
		public async Task<IActionResult> Read([FromRoute] string id)
		{
			var application = await this.applicationService.ReadApplication(id);
			if (application != null)
			{
				return new OkObjectResult(new ApplicationDto(application));
			}

			return new NotFoundResult();
		}

		/// <summary>
		///   Read all applications.
		/// </summary>
		/// <returns>An <see cref="OkObjectResult" /> including all known applications.</returns>
		[HttpGet]
		[Route("all")]
		[CustomAuth(AuthRole.Reader, AuthRole.Writer, AuthRole.Admin)]
		public async Task<IActionResult> Read()
		{
			var applications = await this.applicationService.ReadApplications();
			return new OkObjectResult(applications.Select(application => new ApplicationDto(application)).ToArray());
		}
	}
}