namespace ShopNothingToBuy.Sdk.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Logging;
	using ShopNothingToBuy.Sdk.Contracts;

	/// <summary>
	///   Base class for database services.
	/// </summary>
	/// <typeparam name="TEntry">Type of the database entry.</typeparam>
	/// <typeparam name="TId">Type of the id of the database entry.</typeparam>
	public abstract class AbstractDatabaseService<TEntry, TId> : IDatabase<TEntry, TId>
		where TEntry : class, IDatabaseEntry<TId>
	{
		/// <summary>
		///   Creates a new instance of <see cref="AbstractDatabaseService{TEntry,TId}" />.
		/// </summary>
		/// <param name="logger">A logger for errors.</param>
		protected AbstractDatabaseService(ILogger<AbstractDatabaseService<TEntry, TId>> logger)
		{
			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		///   Gets the error logger.
		/// </summary>
		protected ILogger<AbstractDatabaseService<TEntry, TId>> Logger { get; }

		/// <summary>
		///   Create a new entry in the database.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Created" /> if the entry is created. <see cref="DatabaseResult.AlreadyExists" /> if
		///   the entry already exists. <see cref="DatabaseResult.Error" /> if an error occurred.
		/// </returns>
		public virtual async Task<DatabaseResult> Create(TEntry entry)
		{
			if (await this.Exists(entry.Id))
			{
				return DatabaseResult.AlreadyExists;
			}

			return await this.HandleError(
				async () => await this.CreateEntry(entry),
				"Error while creating database entry.",
				DatabaseResult.Error);
		}

		/// <summary>
		///   Delete an entry with given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Deleted" /> if the entry with <paramref name="id" /> is deleted.
		///   <see cref="DatabaseResult.NotFound" /> if no entry exists with given id. <see cref="DatabaseResult.Error" /> if an
		///   error occurred.
		/// </returns>
		public virtual async Task<DatabaseResult> Delete(TId id)
		{
			if (!await this.Exists(id))
			{
				return DatabaseResult.NotFound;
			}

			return await this.HandleError(
				async () => await this.DeleteEntry(id),
				"Error while deleting database entry.",
				DatabaseResult.Error);
		}

		/// <summary>
		///   Delete all entries from the database.
		/// </summary>
		/// <returns>
		///   <see cref="DatabaseResult.Deleted" /> if all entries are deleted. <see cref="DatabaseResult.Error" /> if an
		///   occurred.
		/// </returns>
		public virtual async Task<DatabaseResult> Delete()
		{
			return await this.HandleError(
				async () => await this.DeleteEntries(),
				"Error while deleting all entries.",
				DatabaseResult.Error);
		}

		/// <summary>
		///   Checks if an entry exists with <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>True if an entry exists and false otherwise.</returns>
		public virtual async Task<bool> Exists(TId id)
		{
			return await this.HandleError(async () => await this.ExistsEntry(id), "Error while checking existence.", false);
		}

		/// <summary>
		///   Read an entry with given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>An instance of <see cref="TEntry" /> if entry with <paramref name="id" /> exists and null otherwise.</returns>
		public virtual async Task<TEntry> Read(TId id)
		{
			return await this.HandleError(async () => await this.ReadEntry(id), "Error while reading entry.", null);
		}

		/// <summary>
		///   Read all entries from the database.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="TEntry" />.</returns>
		public virtual async Task<IEnumerable<TEntry>> Read()
		{
			return await this.HandleError(
				async () => await this.ReadEntries(),
				"Error while reading all entries.",
				Enumerable.Empty<TEntry>());
		}

		/// <summary>
		///   Update a database entry.
		/// </summary>
		/// <param name="entry">The entry to be updated.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Updated" /> if the operation succeeds. <see cref="DatabaseResult.NotFound" /> if no
		///   matching entry in database exists. <see cref="DatabaseResult.Error" /> if an error occurred.
		/// </returns>
		public virtual async Task<DatabaseResult> Update(TEntry entry)
		{
			return await this.HandleError(
				async () => await this.UpdateEntry(entry),
				"Error while updating entry.",
				DatabaseResult.Error);
		}

		/// <summary>
		///   Create a new entry in the database.
		/// </summary>
		/// <param name="entry">The entry to be created.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Created" /> if the entry is created. <see cref="DatabaseResult.AlreadyExists" /> if
		///   the entry already exists. <see cref="DatabaseResult.Error" /> if an error occurred.
		/// </returns>
		protected abstract Task<DatabaseResult> CreateEntry(TEntry entry);

		/// <summary>
		///   Delete all entries from the database.
		/// </summary>
		/// <returns>
		///   <see cref="DatabaseResult.Deleted" /> if all entries are deleted. <see cref="DatabaseResult.Error" /> if an
		///   occurred.
		/// </returns>
		protected virtual async Task<DatabaseResult> DeleteEntries()
		{
			foreach (var databaseEntry in await this.Read())
			{
				var result = await this.Delete(databaseEntry.Id);
				if (result != DatabaseResult.Deleted)
				{
					return result;
				}
			}

			return DatabaseResult.Deleted;
		}

		/// <summary>
		///   Delete an entry with given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Deleted" /> if the entry with <paramref name="id" /> is deleted.
		///   <see cref="DatabaseResult.NotFound" /> if no entry exists with given id. <see cref="DatabaseResult.Error" /> if an
		///   error occurred.
		/// </returns>
		protected abstract Task<DatabaseResult> DeleteEntry(TId id);

		/// <summary>
		///   Checks if an entry exists with <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>True if an entry exists and false otherwise.</returns>
		protected abstract Task<bool> ExistsEntry(TId id);

		/// <summary>
		///   Executes <paramref name="operation" /> in a try-catch-block. In case of an error logs the exception and the
		///   <paramref name="errorMessage" />.
		/// </summary>
		/// <typeparam name="T">The return type of the <paramref name="operation" />.</typeparam>
		/// <param name="operation">The operation to be executed.</param>
		/// <param name="errorMessage">The logged error message if an exception is thrown by <paramref name="operation" />.</param>
		/// <param name="errorResult">The return value of the method in case of a thrown exception.</param>
		/// <returns>The result of <paramref name="operation" /> or the <paramref name="errorResult" />.</returns>
		protected virtual async Task<T> HandleError<T>(Func<Task<T>> operation, string errorMessage, T errorResult)
		{
			try
			{
				return await operation();
			}
			catch (Exception e)
			{
				this.Logger.LogError(e, errorMessage);
				return errorResult;
			}
		}

		/// <summary>
		///   Read all entries from the database.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="TEntry" />.</returns>
		protected abstract Task<IEnumerable<TEntry>> ReadEntries();

		/// <summary>
		///   Read an entry with given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The id of the entry.</param>
		/// <returns>An instance of <see cref="TEntry" /> if entry with <paramref name="id" /> exists and null otherwise.</returns>
		protected abstract Task<TEntry> ReadEntry(TId id);

		/// <summary>
		///   Update a database entry.
		/// </summary>
		/// <param name="entry">The entry to be updated.</param>
		/// <returns>
		///   <see cref="DatabaseResult.Updated" /> if the operation succeeds. <see cref="DatabaseResult.NotFound" /> if no
		///   matching entry in database exists. <see cref="DatabaseResult.Error" /> if an error occurred.
		/// </returns>
		protected abstract Task<DatabaseResult> UpdateEntry(TEntry entry);
	}
}