namespace User.Services
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using User.Services.Models;

	/// <summary>
	///   Authenticate
	/// </summary>
	public class UserService : ServiceBase<UserEntry, string, CreateUserEntry, UpdateUserEntry>
	{
		public UserService(ILogger logger)
			: this(logger, new UserValidator())
		{
		}

		public UserService(ILogger logger, IEntryValidator<CreateUserEntry, UpdateUserEntry, string> validator)
			: base(logger, validator)
		{
		}

		protected override Task<ClearResult> ClearEntries()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		protected override Task<IOperationResult<UserEntry, string, CreateResult>> CreateEntry(CreateUserEntry entry)
		{
			IOperationResult<UserEntry, string, CreateResult> result =
				new OperationResult<UserEntry, string, CreateResult>(
					CreateResult.Created,
					new UserEntry(Guid.NewGuid().ToString()));
			return Task.FromResult(result);
		}

		protected override Task<IOperationResult<UserEntry, string, DeleteResult>> DeleteEntry(string entryId)
		{
			IOperationResult<UserEntry, string, DeleteResult> result =
				new OperationResult<UserEntry, string, DeleteResult>(DeleteResult.Deleted, new UserEntry(entryId));
			return Task.FromResult(result);
		}

		protected override Task<IOperationResult<UserEntry, string, ReadResult>> ReadEntry(string entryId)
		{
			IOperationResult<UserEntry, string, ReadResult> result =
				new OperationResult<UserEntry, string, ReadResult>(ReadResult.Read, new UserEntry(entryId));
			return Task.FromResult(result);
		}

		protected override Task<IOperationResult<UserEntry, string, UpdateResult>> UpdateEntry(UpdateUserEntry entry)
		{
			IOperationResult<UserEntry, string, UpdateResult> result =
				new OperationResult<UserEntry, string, UpdateResult>(UpdateResult.Updated, new UserEntry(entry.Id));
			return Task.FromResult(result);
		}
	}
}