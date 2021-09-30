namespace User.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Application.Contracts;
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
				new MongoDbService<UserEntry, string>(
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
		///   Delete an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		public override Task<IOperationResult<UserEntry, string, DeleteResult>> Delete(string entryId)
		{
			return base.Delete(entryId?.ToUpper());
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		public override Task<ExistsResult> Exists(string entryId)
		{
			return base.Exists(entryId?.ToUpper());
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		public override Task<IOperationResult<UserEntry, string, ReadResult>> Read(string entryId)
		{
			return base.Read(entryId?.ToUpper());
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
			var applcationReadResult = await this.applicationService.Read(entry.ApplicationId);
			if (applcationReadResult.Result == ReadResult.Read
			    && (applcationReadResult.Entry.Roles & entry.Roles) == entry.Roles)
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
			var userEntry = new UserEntry(entry);
			var applcationReadResult = await this.applicationService.Read(entry.ApplicationId);
			if (applcationReadResult.Result == ReadResult.Read
			    && (applcationReadResult.Entry.Roles & entry.Roles) == entry.Roles)
			{
				return await this.DatabaseService.Update(userEntry);
			}

			return new OperationResult<UserEntry, string, UpdateResult>(UpdateResult.InvalidData);
		}

		private async Task<bool> ValidateApplication(UserApplicationEntry application)
		{
			var applicationReadResult = await this.applicationService.Read(application.ApplicationId);
			return applicationReadResult.Result == ReadResult.Read
			       && (applicationReadResult.Entry.Roles & application.Roles) == application.Roles;
		}

		private async Task<bool> ValidateApplications(IEnumerable<UserApplicationEntry> applications)
		{
			return applications.All(async application => await this.ValidateApplication(application));
		}
	}
}