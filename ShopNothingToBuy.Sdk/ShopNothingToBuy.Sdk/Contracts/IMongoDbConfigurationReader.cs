namespace ShopNothingToBuy.Sdk.Contracts
{
	/// <summary>
	///   Read the mongodb configuration.
	/// </summary>
	public interface IMongoDbConfigurationReader
	{
		/// <summary>
		///   Reads the mongodb configuration.
		/// </summary>
		/// <returns>An instance of <see cref="IMongoDbConfiguration" />.</returns>
		IMongoDbConfiguration Read();
	}
}