namespace Application.Api
{
	using Application.Services;
	using Google.Cloud.Functions.Hosting;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Application;
	using Service.Sdk.Contracts.Crud.Database;
	using Service.Sdk.Models.Crud.Database;

	/// <summary>
	///   Entry point of the google cloud function at startup.
	/// </summary>
	public class Startup : FunctionsStartup
	{
		/// <summary>
		///   Name of the configuration entry for mongo database.
		/// </summary>
		private const string MongoDbConfiguration = "MongoDb";

		/// <summary>
		///   Initialize dependencies.
		/// </summary>
		/// <param name="context">The <see cref="WebHostBuilderContext" />.</param>
		/// <param name="services">The <see cref="IServiceCollection" />.</param>
		public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
		{
			var mongoDbConfiguration = new MongoDbConfiguration();
			context.Configuration.Bind(MongoDbConfiguration, mongoDbConfiguration);
			services.AddSingleton<IMongoDbConfiguration>(mongoDbConfiguration);

			services.AddSingleton<ILogger, Logger>();
			services.AddSingleton<IApplicationService, ApplicationService>();
		}
	}
}