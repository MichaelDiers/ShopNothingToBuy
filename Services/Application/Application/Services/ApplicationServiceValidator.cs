namespace Application.Services
{
	using System;
	using System.Threading.Tasks;
	using Application.Contracts;
	using Service.Sdk.Contracts;

	/// <summary>
	///   Validator for <see cref="ApplicationService" /> input data.
	/// </summary>
	public class ApplicationServiceValidator : IEntryValidator<CreateApplicationEntry, UpdateApplicationEntry, string>
	{
		/// <summary>
		///   The max length to application ids.
		/// </summary>
		private const int IdMaxLength = 50;

		/// <summary>
		///   The min length for application ids.
		/// </summary>
		private const int IdMinLength = 3;

		/// <summary>
		///   Validates input data for create operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public async Task<bool> ValidateCreateEntry(CreateApplicationEntry entry)
		{
			return entry != null && await ValidateEntry(entry);
		}

		/// <summary>
		///   Validates the id of an application.
		/// </summary>
		/// <param name="entryId">The id of an entry.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the <paramref name="entryId" /> is valid and false otherwise.</returns>
		public Task<bool> ValidateEntryId(string entryId)
		{
			var result = !string.IsNullOrWhiteSpace(entryId)
			             && entryId.ToUpper() == entryId
			             && entryId.Length >= IdMinLength
			             && entryId.Length <= IdMaxLength;
			return Task.FromResult(result);
		}

		/// <summary>
		///   Validates input data for update operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public async Task<bool> ValidateUpdateEntry(UpdateApplicationEntry entry)
		{
			return entry != null
			       && await ValidateEntry(entry)
			       && !string.IsNullOrWhiteSpace(entry.OriginalId)
			       && string.Equals(entry.Id, entry.OriginalId, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		///   Validate the given <paramref name="baseApplicationEntry" />.
		/// </summary>
		/// <param name="baseApplicationEntry">The entry to be validated.</param>
		/// <returns>True if the entry is valid and false otherwise.</returns>
		private static Task<bool> ValidateEntry(BaseApplicationEntry baseApplicationEntry)
		{
			var result = baseApplicationEntry != null
			             && !string.IsNullOrWhiteSpace(baseApplicationEntry.Id)
			             && baseApplicationEntry.Id.Length >= IdMinLength
			             && baseApplicationEntry.Id.Length <= IdMaxLength
			             && baseApplicationEntry.Roles != Roles.None;
			return Task.FromResult(result);
		}
	}
}