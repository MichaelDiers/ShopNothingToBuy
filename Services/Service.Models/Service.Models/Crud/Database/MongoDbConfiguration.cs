namespace Service.Models.Crud.Database
{
	using Service.Contracts.Crud.Database;

	/// <summary>
	///   Configuration for MongoDb connections.
	/// </summary>
	public class MongoDbConfiguration : IMongoDbConfiguration
	{
		/// <summary>
		///   Gets or sets the mongodb connection string.
		/// </summary>
		public string ConnectionStringFormat { get; set; }

		/// <summary>
		///   Gets or sets the name of the database collection.
		/// </summary>
		public string CollectionName { get; set; }

		/// <summary>
		///   Gets the formatted connection string.
		/// </summary>
		public string ConnectionString => string.Format(this.ConnectionStringFormat, this.DatabaseName);

		/// <summary>
		///   Gets or set the name of the database.
		/// </summary>
		public string DatabaseName { get; set; }
	}
}