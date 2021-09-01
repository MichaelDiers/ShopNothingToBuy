namespace Authentication.Services
{
	using System;
	using System.Threading.Tasks;
	using Authentication.Services.Models;
	using Service.Sdk.Contracts;

	/// <summary>
	///   Validator for the input data of the <see cref="AuthenticationService" />.
	/// </summary>
	public class UserValidator : IEntryValidator<CreateUser, UpdateUser, string>
	{
		/// <summary>
		///   Validates input data for create operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public Task<bool> ValidateCreateEntry(CreateUser entry)
		{
			return Task.FromResult(entry != null);
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
		///   Validates input data for update operations. Uses <see cref="ValidateEntryId" /> for the <see cref="UpdateUser.Id" />
		///   validation.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public async Task<bool> ValidateUpdateEntry(UpdateUser entry)
		{
			var result = entry != null && await this.ValidateEntryId(entry.Id);
			return result;
		}
	}
}