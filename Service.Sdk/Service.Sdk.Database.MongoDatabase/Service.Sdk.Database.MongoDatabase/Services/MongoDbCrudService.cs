namespace Service.Sdk.Database.MongoDatabase.Services
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using MongoDB.Bson;
	using MongoDB.Driver;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Contracts.Crud.Database;
	using Service.Sdk.Services;
	using DeleteResult = Service.Sdk.Contracts.Crud.Base.DeleteResult;
	using UpdateResult = Service.Sdk.Contracts.Crud.Base.UpdateResult;

	/// <summary>
	///   MongoDb implementation as a <see cref="DatabaseService{TEntry,TEntryId}" />.
	/// </summary>
	/// <typeparam name="TEntry">The type of the database entries.</typeparam>
	/// <typeparam name="TEntryId">The type of the database entry id.</typeparam>
	public class MongoDbCrudService<TEntry, TEntryId> : DatabaseService<TEntry, TEntryId>
		where TEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   The name of the id field in the database.
		/// </summary>
		private const string DatabaseNameId = nameof(IEntry<TEntryId>.Id);

		/// <summary>
		///   The MongoDb configuration.
		/// </summary>
		private readonly IMongoDbConfiguration mongoDbConfiguration;

		/// <summary>
		///   The MongoDb client.
		/// </summary>
		private MongoClient mongoClient;

		/// <summary>
		///   Creates a new instance of <see cref="MongoDbCrudService{TEntry,TEntryId}" />.
		/// </summary>
		/// <param name="logger">The logger used for error messages.</param>
		/// <param name="validator">The validator for input data.</param>
		/// <param name="mongoDbConfiguration">The MongoDb configuration for accessing the database.</param>
		public MongoDbCrudService(
			ILogger logger,
			IEntryValidator<TEntry, TEntry, TEntryId> validator,
			IMongoDbConfiguration mongoDbConfiguration)
			: base(logger, validator)
		{
			this.mongoDbConfiguration = mongoDbConfiguration ?? throw new ArgumentNullException(nameof(mongoDbConfiguration));
		}

		/// <summary>
		///   Delete all known entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		protected override async Task<ClearResult> ClearEntries()
		{
			var result = await this.GetCollection().DeleteManyAsync(new BsonDocument());
			return result.IsAcknowledged ? ClearResult.Cleared : ClearResult.InternalError;
		}

		/// <summary>
		///   Create a new entry without previous existence check.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, CreateResult>> CreateNewEntry(TEntry entry)
		{
			await this.GetCollection().InsertOneAsync(entry);
			return new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.Created, entry);
		}

		/// <summary>
		///   Delete an entry by its id without a previous existence check.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="MongoDB.Driver.DeleteResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, DeleteResult>> DeleteExistingEntry(
			TEntryId entryId)
		{
			var readResult = await this.Read(entryId);
			var filter = Builders<TEntry>.Filter.Eq(DatabaseNameId, entryId);
			var result = await this.GetCollection().DeleteOneAsync(filter);
			return result.IsAcknowledged && result.DeletedCount == 1
				? new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.Deleted, readResult.Entry)
				: new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.NotFound);
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		protected override async Task<ExistsResult> ExistsEntry(TEntryId entryId)
		{
			var filter = Builders<TEntry>.Filter.Eq(DatabaseNameId, entryId);
			var count = await this.GetCollection().Find(filter).Limit(1)
				.CountDocumentsAsync();
			return count > 0 ? ExistsResult.Exists : ExistsResult.NotFound;
		}

		/// <summary>
		///   List the ids of all entries.
		/// </summary>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationListResult{T,TOperationResult}" />
		///   that contains the <see cref="ListResult" />.
		/// </returns>
		protected override async Task<IOperationListResult<TEntryId, ListResult>> ListEntries()
		{
			var projection = Builders<TEntry>.Projection.Include(DatabaseNameId);
			var result = await this.GetCollection().Find(new BsonDocument()).Project<TEntry>(projection).ToListAsync();
			return new OperationListResult<TEntryId, ListResult>(
				ListResult.Completed,
				result.Where(entry => entry != null && entry.Id != null).Select(entry => entry.Id).ToArray());
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, ReadResult>> ReadEntry(TEntryId entryId)
		{
			var filter = Builders<TEntry>.Filter.Eq(DatabaseNameId, entryId);
			var entry = await this.GetCollection().Find(filter).FirstOrDefaultAsync();
			return new OperationResult<TEntry, TEntryId, ReadResult>(
				entry != null ? ReadResult.Read : ReadResult.NotFound,
				entry);
		}

		/// <summary>
		///   Update an entry without a previous existence check.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="MongoDB.Driver.UpdateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<TEntry, TEntryId, UpdateResult>> UpdateExistingEntry(TEntry entry)
		{
			var filter = Builders<TEntry>.Filter.Eq(DatabaseNameId, entry.Id);
			var result = await this.GetCollection().ReplaceOneAsync(
				filter,
				entry,
				new ReplaceOptions
				{
					IsUpsert = false
				});
			return result.IsAcknowledged && result.IsModifiedCountAvailable && result.ModifiedCount == 1
				? new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.Updated, entry)
				: new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.NotFound);
		}

		/// <summary>
		///   Opens a connection to the database is necessary and returns the associated collection.
		/// </summary>
		/// <returns>An <see cref="IMongoCollection{TDocument}" />.</returns>
		private IMongoCollection<TEntry> GetCollection()
		{
			this.mongoClient ??= new MongoClient(this.mongoDbConfiguration.ConnectionString);

			return this.mongoClient.GetDatabase(this.mongoDbConfiguration.DatabaseName)
				.GetCollection<TEntry>(this.mongoDbConfiguration.CollectionName);
		}
	}
}