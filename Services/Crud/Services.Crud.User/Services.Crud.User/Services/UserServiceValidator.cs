namespace Services.Crud.User.Services
{
	using System.Linq;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Crud.Application;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Contracts.Crud.User;

	/// <summary>
	///   Validator for the input data of the <see cref="UserService" />.
	/// </summary>
	public class UserServiceValidator : IEntryValidator<CreateUserEntry, UpdateUserEntry, string>
	{
		/// <summary>
		///   Max length of the id.
		/// </summary>
		private const int EntryIdMaxLength = 50;

		/// <summary>
		///   Min length of the id.
		/// </summary>
		private const int EntryIdMinLength = 3;

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
		///   Validates the id of the user entry.
		/// </summary>
		/// <param name="entryId">The id of an entry.</param>
		/// <returns>A <see cref="Task" /> whose result is true if the <paramref name="entryId" /> is valid and false otherwise.</returns>
		public Task<bool> ValidateEntryId(string entryId)
		{
			var result = !string.IsNullOrWhiteSpace(entryId)
			             && entryId.Length >= EntryIdMinLength
			             && entryId.Length <= EntryIdMaxLength;
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
			return entry != null && await this.ValidateEntry(entry);
		}

		/// <summary>
		///   Validate the data of the <see cref="BaseUserEntry" />.
		/// </summary>
		/// <param name="baseUser">The user to be validated.</param>
		/// <returns>A <see cref="Task" /> whose result is a bool.</returns>
		private async Task<bool> ValidateEntry(BaseUserEntry baseUser)
		{
			return baseUser != null
			       && await this.ValidateEntryId(baseUser.Id)
			       && baseUser.Applications != null
			       && baseUser.Applications.All(
				       applicationEntry =>
					       !string.IsNullOrWhiteSpace(applicationEntry.ApplicationId) && applicationEntry.Roles != Roles.None);
		}
	}
}