namespace AuthApi.Models
{
	using System.Linq;
	using AuthApi.Contracts;
	using ShopNothingToBuy.Sdk.Attributes;

	/// <summary>
	///   Data-transfer object for an application.
	/// </summary>
	public class ApplicationDto : CreateApplicationDto<RoleDto>
	{
		/// <summary>
		///   Creates a new instance of <see cref="ApplicationDto" />.
		/// </summary>
		public ApplicationDto()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="ApplicationDto" />.
		/// </summary>
		/// <param name="application">Data is initialized by using the given application.</param>
		public ApplicationDto(IApplication application)
		{
			this.Id = application.Id;
			this.Name = application.Name;
			this.Roles = application.Roles.Select(role => new RoleDto(role)).ToArray();
		}

		/// <summary>
		///   Gets or sets the application id.
		/// </summary>
		[IsGuid(false, false)]
		public string Id { get; set; }
	}
}