namespace Service.Sdk.Contracts.Crud.Database
{
	/// <summary>
	///   MongoDb configuration specification.
	/// </summary>
	public interface IMongoDbConfiguration
	{
		/// <summary>
		///   Gets the name of the database collection.
		/// </summary>
		string CollectionName { get; }

		/// <summary>
		///   Gets the mongodb connection string.
		/// </summary>
		string ConnectionString { get; }

		/// <summary>
		///   Gets the name of the database.
		/// </summary>
		string DatabaseName { get; }
	}
}