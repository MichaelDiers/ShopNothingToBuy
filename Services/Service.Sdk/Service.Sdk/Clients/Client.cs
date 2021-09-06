namespace Service.Sdk.Clients
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;

	/// <summary>
	///   Base class for for clients calling a service.
	/// </summary>
	/// <typeparam name="TEntry">The type of the object to process.</typeparam>
	/// <typeparam name="TEntryId">The type of the id of an entry.</typeparam>
	/// <typeparam name="TCreateEntry">The type of an entry in create operation context.</typeparam>
	/// <typeparam name="TUpdateEntry">The type of an entry in update operation context.</typeparam>
	public class Client<TEntry, TEntryId, TCreateEntry, TUpdateEntry> : ClientBase,
		IServiceBase<TEntry, TEntryId, TCreateEntry, TUpdateEntry>
		where TEntry : class, IEntry<TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   The url of the service.
		/// </summary>
		private readonly string requestUrl;

		/// <summary>
		///   Creates a new instance of <see cref="Client{TEntry,TEntryId,TCreateEntry,TUpdateEntry}" />.
		/// </summary>
		/// <param name="logger">A logger for error messages.</param>
		/// <param name="requestUrl">The url of the service.</param>
		public Client(ILogger logger, string requestUrl)
			: this(logger, null, requestUrl)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="Client{TEntry,TEntryId,TCreateEntry,TUpdateEntry}" />.
		/// </summary>
		/// <param name="logger">A logger for error messages.</param>
		/// <param name="apiKey">The api key used in the service call.</param>
		/// <param name="requestUrl">The url of the service.</param>
		public Client(ILogger logger, string apiKey, string requestUrl)
			: base(logger, apiKey)
		{
			if (string.IsNullOrWhiteSpace(requestUrl))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(requestUrl));
			}

			this.requestUrl = requestUrl;
		}

		/// <summary>
		///   Delete all known entries.
		/// </summary>
		/// <returns>A <see cref="Task" /> whose result is a <see cref="ClearResult" />.</returns>
		public async Task<ClearResult> Clear()
		{
			var result = await this.ServiceCall(this.requestUrl, HttpMethod.Delete.Method);
			return result.StatusCode switch
			{
				HttpStatusCode.NoContent => ClearResult.Cleared,
				HttpStatusCode.Unauthorized => ClearResult.Unauthorized,
				HttpStatusCode.InternalServerError => ClearResult.InternalError,
				_ => throw new ArgumentOutOfRangeException(
					nameof(result.StatusCode),
					result.StatusCode,
					"Unhandled result code.")
			};
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
			var result =
				await this.ServiceCallWithResult<TCreateEntry, TEntry>(this.requestUrl, HttpMethod.Post.Method, entry);
			return result.StatusCode switch
			{
				HttpStatusCode.Created => new OperationResult<TEntry, TEntryId, CreateResult>(
					CreateResult.Created,
					result.ResponseData),
				HttpStatusCode.Conflict => new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.AlreadyExists),
				HttpStatusCode.BadRequest => new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.InvalidData),
				HttpStatusCode.InternalServerError => new OperationResult<TEntry, TEntryId, CreateResult>(
					CreateResult.InternalError),
				HttpStatusCode.Unauthorized => new OperationResult<TEntry, TEntryId, CreateResult>(CreateResult.Unauthorized),
				_ => throw new ArgumentOutOfRangeException(
					nameof(result.StatusCode),
					result.StatusCode,
					"Unhandled result code.")
			};
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
			var result = await this.ServiceCallWithResult<TEntry>($"{this.requestUrl}/{entryId}", HttpMethod.Delete.Method);
			return result.StatusCode switch
			{
				HttpStatusCode.NoContent => new OperationResult<TEntry, TEntryId, DeleteResult>(
					DeleteResult.Deleted,
					result.ResponseData),
				HttpStatusCode.NotFound => new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.NotFound),
				HttpStatusCode.BadRequest => new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.InvalidData),
				HttpStatusCode.InternalServerError => new OperationResult<TEntry, TEntryId, DeleteResult>(
					DeleteResult.InternalError),
				HttpStatusCode.Unauthorized => new OperationResult<TEntry, TEntryId, DeleteResult>(DeleteResult.Unauthorized),
				_ => throw new ArgumentOutOfRangeException(
					nameof(result.StatusCode),
					result.StatusCode,
					"Unhandled result code.")
			};
		}

		/// <summary>
		///   Check if an entry exists.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>
		///   A <see cref="Task" /> whose result is a <see cref="ExistsResult" />.
		/// </returns>
		public async Task<ExistsResult> Exists(TEntryId entryId)
		{
			var result = await this.ServiceCall($"{this.requestUrl}/{entryId}", HttpMethod.Post.Method);
			return result.StatusCode switch
			{
				HttpStatusCode.OK => ExistsResult.Exists,
				HttpStatusCode.NotFound => ExistsResult.NotFound,
				HttpStatusCode.BadRequest => ExistsResult.InvalidData,
				HttpStatusCode.InternalServerError => ExistsResult.InternalError,
				HttpStatusCode.Unauthorized => ExistsResult.Unauthorized,
				_ => throw new ArgumentOutOfRangeException(
					nameof(result.StatusCode),
					result.StatusCode,
					"Unhandled result code.")
			};
		}

		/// <summary>
		///   List the ids of all entries.
		/// </summary>
		/// <returns>
		///   A <see cref="Task" /> whose result is an <see cref="IOperationListResult{T,TOperationResult}" />
		///   that contains the <see cref="ListResult" />.
		/// </returns>
		public async Task<IOperationListResult<TEntryId, ListResult>> List()
		{
			var result = await this.ServiceCallWithResult<IEnumerable<TEntryId>>(this.requestUrl, HttpMethod.Get.Method);
			return result.StatusCode switch
			{
				HttpStatusCode.OK => new OperationListResult<TEntryId, ListResult>(ListResult.Completed, result.ResponseData),
				HttpStatusCode.InternalServerError => new OperationListResult<TEntryId, ListResult>(ListResult.InternalError),
				HttpStatusCode.Unauthorized => new OperationListResult<TEntryId, ListResult>(ListResult.Unauthorized),
				_ => throw new ArgumentOutOfRangeException(
					nameof(result.StatusCode),
					result.StatusCode,
					"Unhandled result code.")
			};
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
			var result = await this.ServiceCallWithResult<TEntry>($"{this.requestUrl}/{entryId}", HttpMethod.Get.Method);
			return result.StatusCode switch
			{
				HttpStatusCode.OK => new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.Read, result.ResponseData),
				HttpStatusCode.NotFound => new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.NotFound),
				HttpStatusCode.BadRequest => new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.InvalidData),
				HttpStatusCode.InternalServerError => new OperationResult<TEntry, TEntryId, ReadResult>(
					ReadResult.InternalError),
				HttpStatusCode.Unauthorized => new OperationResult<TEntry, TEntryId, ReadResult>(ReadResult.Unauthorized),
				_ => throw new ArgumentOutOfRangeException(
					nameof(result.StatusCode),
					result.StatusCode,
					"Unhandled result code.")
			};
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
			var result =
				await this.ServiceCallWithResult<TUpdateEntry, TEntry>(this.requestUrl, HttpMethod.Put.Method, entry);
			return result.StatusCode switch
			{
				HttpStatusCode.OK => new OperationResult<TEntry, TEntryId, UpdateResult>(
					UpdateResult.Updated,
					result.ResponseData),
				HttpStatusCode.NotFound => new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.NotFound),
				HttpStatusCode.BadRequest => new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.InvalidData),
				HttpStatusCode.InternalServerError => new OperationResult<TEntry, TEntryId, UpdateResult>(
					UpdateResult.InternalError),
				HttpStatusCode.Unauthorized => new OperationResult<TEntry, TEntryId, UpdateResult>(UpdateResult.Unauthorized),
				_ => throw new ArgumentOutOfRangeException(
					nameof(result.StatusCode),
					result.StatusCode,
					"Unhandled result code.")
			};
		}
	}
}