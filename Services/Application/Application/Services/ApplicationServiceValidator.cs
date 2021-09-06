namespace Application.Services
{
	using System;
	using System.Threading.Tasks;
	using Application.Services.Models;
	using Service.Sdk.Contracts;

	/// <summary>
	///   Validator for <see cref="ApplicationService" /> input data.
	/// </summary>
	public class ApplicationServiceValidator : IEntryValidator<CreateApplicationEntry, UpdateApplicationEntry, string>
	{
		/// <summary>
		///   Validates input data for create operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public Task<bool> ValidateCreateEntry(CreateApplicationEntry entry)
		{
			var result = entry != null && !string.IsNullOrWhiteSpace(entry.Name);
			return Task.FromResult(result);
		}

		/// <summary>
		///   The <paramref name="entryId" /> is accepted if is a valid <see cref="Guid" />.
		/// </summary>
		/// <param name="entryId">The id of an entry.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the <paramref name="entryId" /> is valid and false otherwise.</returns>
		public Task<bool> ValidateEntryId(string entryId)
		{
			var result = !string.IsNullOrWhiteSpace(entryId) && Guid.TryParse(entryId, out var guid) && guid != Guid.Empty;
			return Task.FromResult(result);
		}

		/// <summary>
		///   Validates input data for update operations. Uses <see cref="ValidateEntryId" /> for the
		///   <see cref="UpdateApplicationEntry.Id" />
		///   validation.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public async Task<bool> ValidateUpdateEntry(UpdateApplicationEntry entry)
		{
			var result = entry != null && await this.ValidateEntryId(entry.Id) && !string.IsNullOrWhiteSpace(entry.Name);
			return result;
		}
	}
}