namespace Service.Sdk.Contracts.Crud.Base
{
	/// <summary>
	///   Base class for entries with an id.
	/// </summary>
	/// <typeparam name="T">The type of the id.</typeparam>
	public class Entry<T> : IEntry<T>
	{
		/// <summary>
		///   Gets or sets the id.
		/// </summary>
		public T Id { get; set; }
	}
}