namespace ProductsApi.Extensions
{
	using Microsoft.Azure.Cosmos.Fluent;
	using Microsoft.Azure.Functions.Extensions.DependencyInjection;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	using ProductsApi.Contracts;
	using ProductsApi.Services;

	/// <summary>
	/// Extensions for <see cref="IFunctionsHostBuilder"/>.
	/// </summary>
	public static class IFunctionsHostBuilderExtensions
	{
		/// <summary>
		/// Name in configuration values of the api key.
		/// </summary>
		private const string apiKeyConfigName = "ApiKey";

		/// <summary>
		/// Name of the config entry for the cosmos connection string.
		/// </summary>
		private const string cosmosDbConnectionString = "CosmosDbConnectionString";

		/// <summary>
		/// Adds the <see cref="IApiKeyService"/> to be available for dependency injection.
		/// </summary>
		/// <param name="builder">An <see cref="IFunctionsHostBuilder"/>.</param>
		public static void AddApiKeyService(this IFunctionsHostBuilder builder)
		{
			builder.Services.AddSingleton<IApiKeyService>((service) =>
			{							
				var configuration = service.GetService<IConfiguration>();
				var apiKey = configuration.GetValue<string>(apiKeyConfigName);
				return new ApiKeyService(apiKey);
			});
		}

		/// <summary>
		/// Adds the <see cref="CosmosClient"/> to be available for dependency injection.
		/// </summary>
		/// <param name="builder">An <see cref="IFunctionsHostBuilder"/>.</param>
		public static void AddCosmosClient(this IFunctionsHostBuilder builder)
		{
			builder.Services.AddSingleton((s) =>
			{
				var configuration = s.GetService<IConfiguration>();
				var connectionString = configuration.GetConnectionString(cosmosDbConnectionString);
				var cosmosClientBuilder = new CosmosClientBuilder(connectionString);
				return cosmosClientBuilder.WithConnectionModeDirect().Build();
			});
		}
	}
}
