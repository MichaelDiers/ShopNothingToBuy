namespace Authentication.Services
{
	using System;
	using System.Threading.Tasks;
	using Authentication.Services.Models;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;

	public class AuthenticationService : ServiceBase<User, string, CreateUser, UpdateUser>
	{
		public AuthenticationService(ILogger logger)
			: this(logger, new UserValidator())
		{
		}

		public AuthenticationService(ILogger logger, IEntryValidator<CreateUser, UpdateUser, string> validator)
			: base(logger, validator)
		{
		}

		protected override Task<ClearResult> ClearEntries()
		{
			return Task.FromResult(ClearResult.Cleared);
		}

		protected override Task<IOperationResult<User, string, CreateResult>> CreateEntry(CreateUser entry)
		{
			IOperationResult<User, string, CreateResult> result =
				new OperationResult<User, string, CreateResult>(CreateResult.Created, new User(Guid.NewGuid().ToString()));
			return Task.FromResult(result);
		}

		protected override Task<IOperationResult<User, string, DeleteResult>> DeleteEntry(string entryId)
		{
			IOperationResult<User, string, DeleteResult> result =
				new OperationResult<User, string, DeleteResult>(DeleteResult.Deleted, new User(entryId));
			return Task.FromResult(result);
		}

		protected override Task<IOperationResult<User, string, ReadResult>> ReadEntry(string entryId)
		{
			IOperationResult<User, string, ReadResult> result =
				new OperationResult<User, string, ReadResult>(ReadResult.Read, new User(entryId));
			return Task.FromResult(result);
		}

		protected override Task<IOperationResult<User, string, UpdateResult>> UpdateEntry(UpdateUser entry)
		{
			IOperationResult<User, string, UpdateResult> result =
				new OperationResult<User, string, UpdateResult>(UpdateResult.Updated, new User(entry.Id));
			return Task.FromResult(result);
		}
	}
}