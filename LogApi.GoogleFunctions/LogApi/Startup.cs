namespace LogApi
{
	using Google.Cloud.Functions.Hosting;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using MongoDB.Bson;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Services;

	/// <summary>
	///   Initialize the google cloud function.
	/// </summary>
	public class Startup : FunctionsStartup
	{
		/// <summary>
		///   Disable logging for the cloud function that logs errors.
		/// </summary>
		/// <param name="context">The <see cref="WebHostBuilderContext" />.</param>
		/// <param name="logging">The <see cref="ILoggingBuilder" />.</param>
		public override void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder logging)
		{
			logging.ClearProviders();
		}

		/// <summary>
		///   Initialize dependencies.
		/// </summary>
		/// <param name="context">The <see cref="WebHostBuilderContext" />.</param>
		/// <param name="services">The <see cref="IServiceCollection" />.</param>
		public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
		{
			services.AddSingleton<IMongoDbConfigurationReader, MongoDbConfigurationReaderAppSettings>();
			services.AddSingleton<IDatabase<LogEntry, ObjectId>, MongoDbService<LogEntry, ObjectId>>();
		}
	}
}