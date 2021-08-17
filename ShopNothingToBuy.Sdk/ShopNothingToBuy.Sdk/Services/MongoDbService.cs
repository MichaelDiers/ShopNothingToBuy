namespace ShopNothingToBuy.Sdk.Services
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;
	using MongoDB.Bson;
	using MongoDB.Driver;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Models;

	/// <summary>
	///   Default MongoDb implementation for CRUD operations.
	/// </summary>
	/// <typeparam name="TEntry">Type of database entries.</typeparam>
	/// <typeparam name="TId">Type of the id of entries.</typeparam>
	public class MongoDbService<TEntry, TId> : AbstractDatabaseService<TEntry, TId>
		where TEntry : class, IDatabaseEntry<TId>
	{
		/// <summary>
		///   Name of the entry in app settings for database configurations.
		/// </summary>
		public const string ConfigurationName = "MongoDb";

		/// <summary>
		///   Connection to the database.
		/// </summary>
		private readonly IMongoDatabase mongoDatabase;

		/// <summary>
		///   Database configuration.
		/// </summary>
		private readonly MongoDbConfiguration mongoDbConfiguration;

		/// <summary>
		///   Creates a new instance of <see cref="MongoDbService{TEntry, TId}" />.
		/// </summary>
		/// <param name="configuration">Access to the configuration of the application.</param>
		/// <param name="logger">A logger for errors.</param>
		public MongoDbService(
			IConfiguration configuration,
			ILogger<AbstractDatabaseService<TEntry, TId>> logger)
			: base(logger)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			this.mongoDbConfiguration = new MongoDbConfiguration();
			configuration.Bind(ConfigurationName, this.mongoDbConfiguration);

			var client = new MongoClient(this.mongoDbConfiguration.ConnectionString);
			this.mongoDatabase = client.GetDatabase(this.mongoDbConfiguration.DatabaseName);
		}

		/// <summary>
		///   Create a new entry in the database.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>The result <see cref="DatabaseResult.Created" />.</returns>
		protected override async Task<DatabaseResult> CreateEntry(TEntry entry)
		{
			await this.GetCollection(this.mongoDbConfiguration.CollectionName).InsertOneAsync(entry);
			return DatabaseResult.Created;
		}

		/// <summary>
		///   Delete an entry from the database.
		/// </summary>
		/// <param name="id">The id of the entry to be deleted.</param>
		/// <returns>
		///   If the operation succeeds <see cref="DatabaseResult.Deleted" /> and <see cref="DatabaseResult.NotFound" />
		///   otherwise.
		/// </returns>
		protected override async Task<DatabaseResult> DeleteEntry(TId id)
		{
			var filter = Builders<TEntry>.Filter.Eq(IDatabase<TEntry, TId>.DatabaseNameId, id);
			var result = await this.GetCollection().DeleteOneAsync(filter);
			return result.IsAcknowledged && result.DeletedCount == 1 ? DatabaseResult.Deleted : DatabaseResult.NotFound;
		}

		/// <summary>
		///   Check if a database entry with <paramref name="id" /> exists.
		/// </summary>
		/// <param name="id">The id of the entry that existence is checked.</param>
		/// <returns>True id an entry with given id exists and false otherwise.</returns>
		protected override async Task<bool> ExistsEntry(TId id)
		{
			var filter = Builders<TEntry>.Filter.Eq(IDatabase<TEntry, TId>.DatabaseNameId, id);
			var count = await this.GetCollection().Find(filter).Limit(1)
				.CountDocumentsAsync();
			return count > 0;
		}

		/// <summary>
		///   Access the collection specified in <see cref="MongoDbConfiguration.CollectionName" />.
		/// </summary>
		/// <returns>An instance of <see cref="IMongoCollection{TDocument}" />.</returns>
		protected virtual IMongoCollection<TEntry> GetCollection()
		{
			return this.GetCollection(this.mongoDbConfiguration.CollectionName);
		}

		/// <summary>
		///   Access the collection with given <paramref name="collectionName" />.
		/// </summary>
		/// <param name="collectionName">The name of the collection.</param>
		/// <returns>An instance of <see cref="IMongoCollection{TDocument}" />.</returns>
		protected virtual IMongoCollection<TEntry> GetCollection(string collectionName)
		{
			return this.mongoDatabase.GetCollection<TEntry>(collectionName);
		}

		/// <summary>
		///   Read all entries of the database collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> containing all database entries.</returns>
		protected override async Task<IEnumerable<TEntry>> ReadEntries()
		{
			var result = await this.GetCollection().FindAsync(new BsonDocument());
			return result.ToList();
		}

		/// <summary>
		///   Read an entry with given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the database entry.</param>
		/// <returns>An instance of <see cref="TEntry" />.</returns>
		protected override async Task<TEntry> ReadEntry(TId id)
		{
			var filter = Builders<TEntry>.Filter.Eq(IDatabase<TEntry, TId>.DatabaseNameId, id);
			var entry = await this.GetCollection().Find(filter).FirstOrDefaultAsync();
			return entry;
		}

		/// <summary>
		///   Update the data of an entry with <see cref="IDatabaseEntry{T}.Id" />
		/// </summary>
		/// <param name="entry">The entry data to be updated.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Updated" /> if the update succeeds and <see cref="DatabaseResult.NotFound" />
		///   otherwise.
		/// </returns>
		protected override async Task<DatabaseResult> UpdateEntry(TEntry entry)
		{
			var filter = Builders<TEntry>.Filter.Eq(IDatabase<TEntry, TId>.DatabaseNameId, entry.Id);
			var result = await this.GetCollection().ReplaceOneAsync(
				filter,
				entry,
				new ReplaceOptions
				{
					IsUpsert = false
				});
			return result.IsAcknowledged && result.IsModifiedCountAvailable && result.ModifiedCount == 1
				? DatabaseResult.Updated
				: DatabaseResult.NotFound;
		}
	}
}