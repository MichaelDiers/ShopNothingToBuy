namespace ShopNothingToBuy.Sdk.Services
{
	using System;
	using Microsoft.Extensions.Configuration;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Models;

	/// <summary>
	///   Read the configuration from the app settings of the application.
	/// </summary>
	public class MongoDbConfigurationReaderAppSettings : IMongoDbConfigurationReader
	{
		/// <summary>
		///   Name of the entry in app settings for database configurations.
		/// </summary>
		public const string ConfigurationName = "MongoDb";

		/// <summary>
		///   The mongoDb configuration.
		/// </summary>
		private readonly IMongoDbConfiguration mongoDbConfiguration;

		/// <summary>
		///   Creates a new instance of <see cref="MongoDbConfigurationReaderAppSettings" />.
		/// </summary>
		/// <param name="configuration">The application configuration.</param>
		public MongoDbConfigurationReaderAppSettings(IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			this.mongoDbConfiguration = new MongoDbConfiguration();
			configuration.Bind(ConfigurationName, this.mongoDbConfiguration);
		}

		/// <summary>
		///   Reads the mongodb configuration.
		/// </summary>
		/// <returns>An instance of <see cref="IMongoDbConfiguration" />.</returns>
		public IMongoDbConfiguration Read()
		{
			return this.mongoDbConfiguration;
		}
	}
}