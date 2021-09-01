namespace Service.Sdk
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;

	/// <summary>
	///   Base class for services.
	/// </summary>
	/// <typeparam name="TEntry">The type of the object to process.</typeparam>
	/// <typeparam name="TEntryId">The type of the id of an entry.</typeparam>
	/// <typeparam name="TCreateEntry">The type of an entry in create operation context.</typeparam>
	/// <typeparam name="TUpdateEntry">The type of an entry in update operation context.</typeparam>
	public abstract class ServiceBase<TEntry, TEntryId, TCreateEntry, TUpdateEntry>
		: IServiceBase<TEntry, TEntryId, TCreateEntry, TUpdateEntry>
		where TEntry : class, IEntry<TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   The error logger of the application.
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		///   The validator for input data.
		/// </summary>
		private readonly IEntryValidator<TCreateEntry, TUpdateEntry, TEntryId> validator;

		/// <summary>
		///   Create a new instance of <see cref="ServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}" />.
		/// </summary>
		/// <param name="logger">The error logger of the application.</param>
		/// <param name="validator">The validator for input data of the application.</param>
		protected ServiceBase(ILogger logger, IEntryValidator<TCreateEntry, TUpdateEntry, TEntryId> validator)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		/// <summary>
		///   Delete all known entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		public async Task<ClearResult> Clear()
		{
			try
			{
				return await this.ClearEntries();
			}
			catch (Exception ex)
			{
				await this.LogError("Error executing clear entries.", ex);
				return ClearResult.InternalError;
			}
		}

		/// <summary>
		///   Create a new entry.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		public async Task<IOperationResult<TEntry, TEntryId, CreateResult>> Create(TCreateEntry entry)
		{
			try
			{
				if (await this.validator.ValidateCreateEntry(entry))
				{
					return await this.CreateEntry(entry);
				}

				return new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.InvalidData);
			}
			catch (Exception ex)
			{
				await this.LogError("Error executing create entry.", ex);
				return new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.InternalError);
			}
		}

		/// <summary>
		///   Delete an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		public async Task<IOperationResult<TEntry, TEntryId, DeleteResult>> Delete(TEntryId entryId)
		{
			try
			{
				if (await this.validator.ValidateEntryId(entryId))
				{
					return await this.DeleteEntry(entryId);
				}

				return new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.InvalidData);
			}
			catch (Exception ex)
			{
				await this.LogError("Error executing delete entry.", ex);
				return new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.InternalError);
			}
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		public async Task<IOperationResult<TEntry, TEntryId, ReadResult>> Read(TEntryId entryId)
		{
			try
			{
				if (await this.validator.ValidateEntryId(entryId))
				{
					return await this.ReadEntry(entryId);
				}

				return new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.InvalidData);
			}
			catch (Exception ex)
			{
				await this.LogError("Error executing read entry.", ex);
				return new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.InternalError);
			}
		}

		/// <summary>
		///   Update an entry.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		public async Task<IOperationResult<TEntry, TEntryId, UpdateResult>> Update(TUpdateEntry entry)
		{
			try
			{
				if (await this.validator.ValidateUpdateEntry(entry))
				{
					return await this.UpdateEntry(entry);
				}

				return new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.InvalidData);
			}
			catch (Exception ex)
			{
				await this.LogError("Error executing update entry.", ex);
				return new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.InternalError);
			}
		}

		/// <summary>
		///   Delete all known entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		protected abstract Task<ClearResult> ClearEntries();

		/// <summary>
		///   Create a new entry.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="CreateResult" />.
		/// </returns>
		protected abstract Task<IOperationResult<TEntry, TEntryId, CreateResult>> CreateEntry(TCreateEntry entry);

		/// <summary>
		///   Delete an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="DeleteResult" />.
		/// </returns>
		protected abstract Task<IOperationResult<TEntry, TEntryId, DeleteResult>> DeleteEntry(TEntryId entryId);

		/// <summary>
		///   Adds the given <paramref name="message" /> to the error log.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected async Task LogError(string message)
		{
			await this.LogError(message, null);
		}

		/// <summary>
		///   Adds the given <paramref name="message" /> and <see cref="Exception" /> <paramref name="ex" /> to the error log.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <param name="ex">The <see cref="Exception" /> to be logged.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected async Task LogError(string message, Exception ex)
		{
			try
			{
				await this.logger.Error(message, ex);
			}
			catch
			{
				// cannot handle logging errors
			}
		}

		/// <summary>
		///   Read an entry by its id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="ReadResult" />.
		/// </returns>
		protected abstract Task<IOperationResult<TEntry, TEntryId, ReadResult>> ReadEntry(TEntryId entryId);

		/// <summary>
		///   Update an entry.
		/// </summary>
		/// <param name="entry">The new values of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationResult{TEntry,TEntryId,TOperationResult}" />
		///   that contains the <see cref="UpdateResult" />.
		/// </returns>
		protected abstract Task<IOperationResult<TEntry, TEntryId, UpdateResult>> UpdateEntry(TUpdateEntry entry);
	}
}