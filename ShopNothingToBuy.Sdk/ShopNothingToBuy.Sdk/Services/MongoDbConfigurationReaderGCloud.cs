namespace ShopNothingToBuy.Sdk.Services
{
	using System;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Models;

	/// <summary>
	///   Read the configuration from google cloud configuration.
	/// </summary>
	public class MongoDbConfigurationReaderGCloud : IMongoDbConfigurationReader
	{
		/// <summary>
		///   Name of the environment variable for the collection name.
		/// </summary>
		private const string MongoDbCollectionName = nameof(MongoDbCollectionName);

		/// <summary>
		///   Name of the environment variable for the connection string.
		/// </summary>
		private const string MongoDbConnectionStringFormat = nameof(MongoDbConnectionStringFormat);

		/// <summary>
		///   Name of the environment variable for the database name.
		/// </summary>
		private const string MongoDbDatabaseName = nameof(MongoDbDatabaseName);

		/// <summary>
		///   The mongoDb configuration.
		/// </summary>
		private IMongoDbConfiguration mongoDbConfiguration;

		/// <summary>
		///   Reads the mongodb configuration.
		/// </summary>
		/// <returns>An instance of <see cref="IMongoDbConfiguration" />.</returns>
		public IMongoDbConfiguration Read()
		{
			if (this.mongoDbConfiguration == null)
			{
				this.mongoDbConfiguration = new MongoDbConfiguration
				{
					CollectionName = Environment.GetEnvironmentVariable(MongoDbCollectionName)
					                 ?? throw new ArgumentNullException(
						                 MongoDbCollectionName,
						                 "Missing configuration entry"),
					DatabaseName = Environment.GetEnvironmentVariable(MongoDbDatabaseName)
					               ?? throw new ArgumentNullException(
						               MongoDbDatabaseName,
						               "Missing configuration entry"),
					ConnectionStringFormat = Environment.GetEnvironmentVariable(MongoDbConnectionStringFormat)
					                         ?? throw new ArgumentNullException(
						                         MongoDbConnectionStringFormat,
						                         "Missing configuration entry")
				};
			}

			return this.mongoDbConfiguration;
		}
	}
}