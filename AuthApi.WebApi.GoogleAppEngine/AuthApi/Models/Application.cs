namespace AuthApi.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AuthApi.Contracts;
	using Google.Cloud.Firestore;

	/// <summary>
	///   Describes an application.
	/// </summary>
	[FirestoreData]
	public class Application : IApplication
	{
		/// <summary>
		///   The entry name in database for the application id.
		/// </summary>
		public const string DatabaseApplicationId = "applicationId";

		/// <summary>
		///   Creates a new instance of <see cref="Application" />.
		/// </summary>
		public Application()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="Application" />.
		/// </summary>
		/// <param name="applicationDto">Data used for initialization.</param>
		public Application(ApplicationDto applicationDto)
		{
			this.Id = applicationDto.Id ?? Guid.NewGuid().ToString();
			this.Name = applicationDto.Name;
			this.Roles = applicationDto.Roles.Select(role => new Role(role)).ToArray();
		}

		/// <summary>
		///   Creates a new instance of <see cref="Application" />.
		/// </summary>
		/// <param name="createApplicationDto">>Data used for initialization.</param>
		public Application(CreateApplicationDto<CreateRoleDto> createApplicationDto)
		{
			this.Id = Guid.NewGuid().ToString();
			this.Name = createApplicationDto.Name;
			this.Roles = createApplicationDto.Roles.Select(role => new Role(role)).ToArray();
		}

		/// <summary>
		///   Gets or sets the application id.
		/// </summary>
		[FirestoreProperty(DatabaseApplicationId)]
		public string Id { get; set; }

		/// <summary>
		///   Gets or sets the name of the application.
		/// </summary>
		[FirestoreProperty("name")]
		public string Name { get; set; }

		/// <summary>
		///   Gets or sets the supported roles of the application.
		/// </summary>
		[FirestoreProperty("roles")]
		public IReadOnlyCollection<Role> Roles { get; set; }
	}
}