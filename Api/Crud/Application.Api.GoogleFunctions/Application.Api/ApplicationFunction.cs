﻿namespace Application.Api
{
	using Google.Cloud.Functions.Framework;
	using Google.Cloud.Functions.Hosting;
	using Service.GoogleCloud.Functions;
	using Service.Sdk.Contracts.Crud.Application;

	/// <summary>
	///   API for CRUD operations on application entries. Uses google cloud functions.
	/// </summary>
	[FunctionsStartup(typeof(Startup))]
	public class ApplicationFunction :
		CrudGoogleFunction<ApplicationEntry, string, CreateApplicationEntry, UpdateApplicationEntry>, IHttpFunction
	{
		/// <summary>
		///   Creates a new instance of <see cref="ApplicationFunction" />.
		/// </summary>
		/// <param name="service">Service for processing application entries.</param>
		public ApplicationFunction(IApplicationService service)
			: base(service)
		{
		}
	}
}