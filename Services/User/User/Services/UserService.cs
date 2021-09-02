namespace User.Services
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using User.Services.Models;

	/// <summary>
	///   Service for creating, reading, updating and deleting users.
	/// </summary>
	public class UserService : ServiceBase<UserEntry, string, CreateUserEntry, UpdateUserEntry>
	{
		/// <summary>
		///   Creates a new instance of <see cref="UserService" />.
		/// </summary>
		/// <param name="logger">A logger for writing error messages.</param>
		public UserService(ILogger logger)
			: this(logger, new UserValidator())
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="UserService" />.
		/// </summary>
		/// <param name="logger">A logger for writing error messages.</param>
		/// <param name="validator">A validator for input service input data.</param>
		public UserService(ILogger logger, IEntryValidator<CreateUserEntry, UpdateUserEntry, string> validator)
			: base(logger, validator)
		{
		}

		/// <summary>
		///   Delete all known user entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		protected override Task<ClearResult> ClearEntries()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		/// <summary>
		///   Create a new user entry.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected override Task<IOperationResult<UserEntry, string, CreateResult>> CreateEntry(CreateUserEntry entry)
		{
			IOperationResult<UserEntry, string, CreateResult> result =
				new OperationResult<UserEntry, string, CreateResult>(
					CreateResult.Created,
					new UserEntry(Guid.NewGuid().ToString()));
			return Task.FromResult(result);
		}

		/// <summary>
		///   Delete an user entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		protected override Task<IOperationResult<UserEntry, string, DeleteResult>> DeleteEntry(string entryId)
		{
			IOperationResult<UserEntry, string, DeleteResult> result =
				new OperationResult<UserEntry, string, DeleteResult>(DeleteResult.Deleted, new UserEntry(entryId));
			return Task.FromResult(result);
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		protected override Task<ExistsResult> ExistsEntry(string entryId)
		{
			return Task.FromResult(ExistsResult.Exists);
		}

		/// <summary>
		///   List the ids of all entries.
		/// </summary>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationListResult{T,TOperationResult}" />
		///   that contains the <see cref="ListResult" />.
		/// </returns>
		protected override Task<IOperationListResult<string, ListResult>> ListEntries()
		{
			var result = new OperationListResult<string, ListResult>(ListResult.Completed, Enumerable.Empty<string>());
			return Task.FromResult(result as IOperationListResult<string, ListResult>);
		}

		/// <summary>
		///   Read an user entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		protected override Task<IOperationResult<UserEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			IOperationResult<UserEntry, string, ReadResult> result =
				new OperationResult<UserEntry, string, ReadResult>(ReadResult.Read, new UserEntry(entryId));
			return Task.FromResult(result);
		}

		/// <summary>
		///   Update an user entry.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected override Task<IOperationResult<UserEntry, string, UpdateResult>> UpdateEntry(UpdateUserEntry entry)
		{
			IOperationResult<UserEntry, string, UpdateResult> result =
				new OperationResult<UserEntry, string, UpdateResult>(UpdateResult.Updated, new UserEntry(entry.Id));
			return Task.FromResult(result);
		}
	}
}