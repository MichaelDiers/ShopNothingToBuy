namespace ShopNothingToBuy.Sdk.Models
{
	using ShopNothingToBuy.Sdk.Contracts;

	/// <summary>
	///   Configuration for MongoDb connections.
	/// </summary>
	public class MongoDbConfiguration : IMongoDbConfiguration
	{
		/// <summary>
		///   Gets or sets the name of the database collection.
		/// </summary>
		public string CollectionName { get; set; }

		/// <summary>
		///   Gets or sets the mongodb connection string.
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		///   Gets or set the name of the database.
		/// </summary>
		public string DatabaseName { get; set; }
	}
}