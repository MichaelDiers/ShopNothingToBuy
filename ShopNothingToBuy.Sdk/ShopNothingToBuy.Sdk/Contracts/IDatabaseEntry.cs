namespace ShopNothingToBuy.Sdk.Contracts
{
	/// <summary>
	///   Describes a database entry.
	/// </summary>
	/// <typeparam name="T">The type of the id of the database entry.</typeparam>
	public interface IDatabaseEntry<out T>
	{
		/// <summary>
		///   Gets the id of the entry.
		/// </summary>
		T Id { get; }
	}
}