namespace Services.Crud.Application.Services
{
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Business.Log;
	using Service.Sdk.Contracts.Crud.Application;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Contracts.Crud.Database;
	using Service.Sdk.Database.MongoDatabase.Services;
	using Service.Sdk.Services;

	/// <summary>
	///   Service for creating, reading, updating and deleting application data.
	/// </summary>
	public class ApplicationService : ServiceDatabaseBaseStringId<
		ApplicationEntry,
		CreateApplicationEntry,
		UpdateApplicationEntry>, IApplicationService
	{
		/// <summary>
		///   Creates a new instance of <see cref="ApplicationService" />.
		/// </summary>
		/// <param name="logger">The logger for error messages.</param>
		/// <param name="mongoDbConfiguration">The MongoDb configuration.</param>
		public ApplicationService(
			ILogger logger,
			IMongoDbConfiguration mongoDbConfiguration)
			: base(
				logger,
				new ApplicationServiceValidator(),
				new MongoDbCrudService<ApplicationEntry, string>(
					logger,
					new DefaultValidator<ApplicationEntry, ApplicationEntry, string>(),
					mongoDbConfiguration))
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="ApplicationService" />.
		/// </summary>
		/// <param name="logger">The logger for error messages.</param>
		/// <param name="validator">THe validator for input data.</param>
		/// <param name="databaseService">The service for accessing the application database.</param>
		public ApplicationService(
			ILogger logger,
			IEntryValidator<CreateApplicationEntry, UpdateApplicationEntry, string> validator,
			IDatabaseService<ApplicationEntry, string> databaseService)
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
		protected override async Task<IOperationResult<ApplicationEntry, string, CreateResult>> CreateEntry(
			CreateApplicationEntry entry)
		{
			var applicationEntry = new ApplicationEntry(entry?.Id?.ToUpper(), entry?.Id, entry?.Roles ?? Roles.None);
			return await this.DatabaseService.Create(applicationEntry);
		}

		/// <summary>
		///   Update an entry.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected override async Task<IOperationResult<ApplicationEntry, string, UpdateResult>> UpdateEntry(
			UpdateApplicationEntry entry)
		{
			var applicationEntry = new ApplicationEntry(entry?.Id?.ToUpper(), entry?.OriginalId, entry?.Roles ?? Roles.None);
			return await this.DatabaseService.Update(applicationEntry);
		}
	}
}