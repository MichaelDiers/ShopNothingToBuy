namespace Service.Sdk.Services
{
	using System.Threading.Tasks;
	using Service.Contracts.Crud.Base;
	using Service.Sdk.Contracts;

	/// <summary>
	///   Validator for service input data. The validation will succeed for any given input data.
	/// </summary>
	/// <typeparam name="TCreateEntry">The input type for create operations.</typeparam>
	/// <typeparam name="TUpdateEntry">The input type for update operations.</typeparam>
	/// <typeparam name="TEntryId">The type for entry ids.</typeparam>
	public class
		DefaultValidator<TCreateEntry, TUpdateEntry, TEntryId> : IEntryValidator<TCreateEntry, TUpdateEntry, TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Validates input data for create operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true.</returns>
		public Task<bool> ValidateCreateEntry(TCreateEntry entry)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		///   Validates the id of an entry.
		/// </summary>
		/// <param name="entryId">The id of an entry.</param>
		/// <returns>A <see cref="Task" /> whose result is true.</returns>
		public Task<bool> ValidateEntryId(TEntryId entryId)
		{
			return Task.FromResult(true);
		}

		/// <summary>
		///   Validates input data for update operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true.</returns>
		public Task<bool> ValidateUpdateEntry(TUpdateEntry entry)
		{
			return Task.FromResult(true);
		}
	}
}