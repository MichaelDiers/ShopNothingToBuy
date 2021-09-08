namespace Application.Services
{
	using System.Threading.Tasks;
	using Application.Contracts;
	using MongoDatabase.Contracts;
	using MongoDatabase.Services;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;

	/// <summary>
	///   Service for creating, reading, updating and deleting application data.
	/// </summary>
	public class ApplicationService : ServiceDatabaseBase<ApplicationEntry, string, CreateApplicationEntry,
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
				new MongoDbService<ApplicationEntry, string>(
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
		///   Delete an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		public override async Task<IOperationResult<ApplicationEntry, string, DeleteResult>> Delete(string entryId)
		{
			return await base.Delete(entryId?.ToUpper());
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		public override async Task<ExistsResult> Exists(string entryId)
		{
			return await base.Exists(entryId?.ToUpper());
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		public override async Task<IOperationResult<ApplicationEntry, string, ReadResult>> Read(string entryId)
		{
			return await base.Read(entryId?.ToUpper());
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
			var applicationEntry = new ApplicationEntry(entry);
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
			var applicationEntry = new ApplicationEntry(entry);
			return await this.DatabaseService.Update(applicationEntry);
		}
	}
}