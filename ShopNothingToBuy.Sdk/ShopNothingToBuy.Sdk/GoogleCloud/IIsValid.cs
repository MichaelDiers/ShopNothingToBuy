namespace ShopNothingToBuy.Sdk.GoogleCloud
{
	/// <summary>
	///   Specifies an object validator.
	/// </summary>
	public interface IIsValid
	{
		/// <summary>
		///   Checks if an object is valid.
		/// </summary>
		/// <returns>True if the object is valid and false otherwise.</returns>
		bool IsValid();
	}
}