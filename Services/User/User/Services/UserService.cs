namespace User.Services
{
	using System.Threading.Tasks;
	using MongoDatabase.Contracts;
	using MongoDatabase.Services;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using User.Contracts;
	using User.Services.Models;

	/// <summary>
	///   Service for creating, reading, updating and deleting users.
	/// </summary>
	public class UserService : ServiceDatabaseBase<UserEntry, string, CreateUserEntry, UpdateUserEntry>, IUserService
	{
		/// <summary>
		///   Creates a new instance of <see cref="UserService" /> using a MongoDb connection.
		/// </summary>
		/// <param name="logger">The logger for error messages.</param>
		/// <param name="mongoDbConfiguration">The configuration of the used MongoDb.</param>
		public UserService(ILogger logger, IMongoDbConfiguration mongoDbConfiguration)
			: this(
				logger,
				new UserServiceValidator(),
				new MongoDbService<UserEntry, string>(
					logger,
					new DefaultValidator<UserEntry, UserEntry, string>(),
					mongoDbConfiguration))
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="UserService" />.
		/// </summary>
		/// <param name="logger">The logger for error messages.</param>
		/// <param name="validator">The validator used for input data.</param>
		/// <param name="databaseService">Access to the user database.</param>
		public UserService(
			ILogger logger,
			IEntryValidator<CreateUserEntry, UpdateUserEntry, string> validator,
			IDatabaseService<UserEntry, string> databaseService)
			: base(logger, validator, databaseService)
		{
		}

		/// <summary>
		///   Create a new entry.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<UserEntry, string, CreateResult>> CreateEntry(CreateUserEntry entry)
		{
			var userEntry = new UserEntry(entry);
			return await this.DatabaseService.Create(userEntry);
		}

		/// <summary>
		///   Update an entry.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<UserEntry, string, UpdateResult>> UpdateEntry(UpdateUserEntry entry)
		{
			var userEntry = new UserEntry(entry);
			return await this.DatabaseService.Update(userEntry);
		}
	}
}