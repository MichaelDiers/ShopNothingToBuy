namespace Service.Contracts.Crud.Base
{
	/// <summary>
	///   Data-transfer object for multple ids.
	/// </summary>
	/// <typeparam name="T">The type of the ids.</typeparam>
	public class Entries<T>
	{
		/// <summary>
		///   Gets or sets the ida.
		/// </summary>
		public T[] Ids { get; set; }
	}
}