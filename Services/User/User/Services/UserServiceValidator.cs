namespace User.Services
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using User.Services.Models;

	/// <summary>
	///   Validator for the input data of the <see cref="UserService" />.
	/// </summary>
	public class UserServiceValidator : IEntryValidator<CreateUserEntry, UpdateUserEntry, string>
	{
		/// <summary>
		///   Validates input data for create operations.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public async Task<bool> ValidateCreateEntry(CreateUserEntry entry)
		{
			return entry != null && await this.ValidateEntry(entry);
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
		///   <see cref="UpdateUserEntry.Id" />
		///   validation.
		/// </summary>
		/// <param name="entry">The input data.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the data is valid and false otherwise.</returns>
		public async Task<bool> ValidateUpdateEntry(UpdateUserEntry entry)
		{
			var result = entry != null && await this.ValidateEntryId(entry.Id) && await this.ValidateEntry(entry);
			return result;
		}

		/// <summary>
		///   Validate the data of the <see cref="BaseUser" />.
		/// </summary>
		/// <param name="baseUser">The user to be validated.</param>
		/// <returns>A <see cref="Task" /> whose result is a bool.</returns>
		private async Task<bool> ValidateEntry(BaseUser baseUser)
		{
			return await this.ValidateEntryId(baseUser.ApplicationId) && !string.IsNullOrWhiteSpace(baseUser.Name);
		}
	}
}