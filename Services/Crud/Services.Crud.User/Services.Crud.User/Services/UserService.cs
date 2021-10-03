namespace Services.Crud.User.Services
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Application;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Contracts.Crud.Database;
	using Service.Sdk.Contracts.Crud.User;
	using Service.Sdk.Database.MongoDatabase.Services;
	using Service.Sdk.Services;

	/// <summary>
	///   Service for creating, reading, updating and deleting users.
	/// </summary>
	public class UserService : ServiceDatabaseBaseStringId<UserEntry, CreateUserEntry, UpdateUserEntry>, IUserService
	{
		/// <summary>
		///   Access to the application service.
		/// </summary>
		private readonly IApplicationService applicationService;

		/// <summary>
		///   Creates a new instance of <see cref="UserService" /> using a MongoDb connection.
		/// </summary>
		/// <param name="logger">The logger for error messages.</param>
		/// <param name="mongoDbConfiguration">The configuration of the used MongoDb.</param>
		/// <param name="applicationService">Service for accessing the known applications.</param>
		public UserService(
			ILogger logger,
			IMongoDbConfiguration mongoDbConfiguration,
			IApplicationService applicationService)
			: this(
				logger,
				new UserServiceValidator(),
				new MongoDbCrudService<UserEntry, string>(
					logger,
					new DefaultValidator<UserEntry, UserEntry, string>(),
					mongoDbConfiguration),
				applicationService)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="UserService" />.
		/// </summary>
		/// <param name="logger">The logger for error messages.</param>
		/// <param name="validator">The validator used for input data.</param>
		/// <param name="databaseService">Access to the user database.</param>
		/// <param name="applicationService">Service for accessing the known applications.</param>
		public UserService(
			ILogger logger,
			IEntryValidator<CreateUserEntry, UpdateUserEntry, string> validator,
			IDatabaseService<UserEntry, string> databaseService,
			IApplicationService applicationService)
			: base(logger, validator, databaseService)
		{
			this.applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
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
			var userEntry = new UserEntry(entry?.Id?.ToUpper(), entry?.Id, entry?.Applications);
			var applicationReadResult = await this.applicationService.Read(
				userEntry.Applications?.Select(application => application.ApplicationId).ToArray());
			var valid = entry?.Applications.Zip(applicationReadResult).All(
				data => data.Second.Result == ReadResult.Read
				        && (data.First.Roles & data.Second.Entry.Roles) == data.First.Roles);
			if (valid == true)
			{
				return await this.DatabaseService.Create(userEntry);
			}

			return new OperationResult<UserEntry, string, CreateResult>(CreateResult.InvalidData);
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
			var userEntry = new UserEntry(entry?.Id?.ToUpper(), entry?.OriginalId, entry?.Applications);
			return await this.DatabaseService.Update(userEntry);
			/*
			var applcationReadResult = await this.applicationService.Read(entry.ApplicationId);
			if (applcationReadResult.Result == ReadResult.Read
			    && (applcationReadResult.Entry.Roles & entry.Roles) == entry.Roles)
			{
				return await this.DatabaseService.Update(userEntry);
			}
			*/
			//return new OperationResult<UserEntry, string, UpdateResult>(UpdateResult.InvalidData);
		}
	}
}