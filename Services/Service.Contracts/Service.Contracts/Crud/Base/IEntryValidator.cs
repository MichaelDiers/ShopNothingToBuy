namespace Service.Contracts.Crud.Base
{
	using System.Threading.Tasks;

	/// <summary>
	///   Validator for service input data.
	/// </summary>
	/// <typeparam name="TCreateEntry">The input type for create operations.</typeparam>
	/// <typeparam name="TUpdateEntry">The input type for update operations.</typeparam>
	/// <typeparam name="TEntryId">The type for entry ids.</typeparam>
	public interface IEntryValidator<in TCreateEntry, in TUpdateEntry, in TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Validates input data for create operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		Task<bool> ValidateCreateEntry(TCreateEntry entry);

		/// <summary>
		///   Validates the id of an entry.
		/// </summary>
		/// <param name="entryId">The id of an entry.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the <paramref name="entryId" /> is valid and false otherwise.</returns>
		Task<bool> ValidateEntryId(TEntryId entryId);

		/// <summary>
		///   Validates input data for update operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		Task<bool> ValidateUpdateEntry(TUpdateEntry entry);
	}
}