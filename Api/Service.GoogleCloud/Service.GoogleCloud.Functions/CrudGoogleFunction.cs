namespace Service.GoogleCloud.Functions
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Newtonsoft.Json;
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Base class for CRUD apis using google functions.
	/// </summary>
	/// <typeparam name="TEntry">The type of the object to process.</typeparam>
	/// <typeparam name="TEntryId">The type of the id of an entry.</typeparam>
	/// <typeparam name="TCreateEntry">The type of an entry in create operation context.</typeparam>
	/// <typeparam name="TUpdateEntry">The type of an entry in update operation context.</typeparam>
	public abstract class CrudGoogleFunction<TEntry, TEntryId, TCreateEntry, TUpdateEntry> : GoogleFunction
		where TEntry : class, IEntry<TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Name of the id field.
		/// </summary>
		protected const string Id = "ID";

		/// <summary>
		///   The service that processes the input data.
		/// </summary>
		private readonly IServiceBase<TEntry, TEntryId, TCreateEntry, TUpdateEntry> service;

		/// <summary>
		///   Creates a new instance of <see cref="CrudGoogleFunction{TEntry,TEntryId,TCreateEntry,TUpdateEntry}" />.
		/// </summary>
		/// <param name="service">The service that processes the input data.</param>
		protected CrudGoogleFunction(IServiceBase<TEntry, TEntryId, TCreateEntry, TUpdateEntry> service)
		{
			this.service = service ?? throw new ArgumentNullException(nameof(service));
		}

		/// <summary>
		///   Handle a http delete request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected override async Task HandleDelete(IRequestData requestData, HttpContext context)
		{
			if (!this.Validate(requestData))
			{
				context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			}
			else if (string.IsNullOrWhiteSpace(requestData.Body) && requestData.Query?.Any() != true)
			{
				await this.HandleClear(requestData, context);
			}
			else if (requestData.Query?.Any() == true)
			{
				await this.HandleDeleteEntry(requestData, context);
			}
			else
			{
				context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			}
		}

		/// <summary>
		///   Handle a http get request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected override async Task HandleGet(IRequestData requestData, HttpContext context)
		{
			if (!this.Validate(requestData))
			{
				context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			}
			else if ((requestData.Query == null || !requestData.Query.Any()) && string.IsNullOrWhiteSpace(requestData.Body))
			{
				await this.HandleList(requestData, context);
			}
			else if (!string.IsNullOrWhiteSpace(requestData.Body))
			{
				await this.HandleGetEntries(requestData, context);
			}
			else
			{
				await this.HandleGetEntry(requestData, context);
			}
		}

		/// <summary>
		///   Handle a http head request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected override async Task HandleHead(IRequestData requestData, HttpContext context)
		{
			if (!this.Validate(requestData)
			    || !string.IsNullOrWhiteSpace(requestData.Body)
			    || requestData.Query?.Any() == false)
			{
				context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			}
			else
			{
				var entryId = (TEntryId) Convert.ChangeType(requestData.Query[Id], typeof(TEntryId));
				var existsResult = await this.service.Exists(entryId);
				switch (existsResult)
				{
					case ExistsResult.Exists:
						context.Response.StatusCode = (int) HttpStatusCode.OK;
						break;
					case ExistsResult.NotFound:
						context.Response.StatusCode = (int) HttpStatusCode.NotFound;
						break;
					case ExistsResult.InvalidData:
						context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
						break;
					case ExistsResult.Unauthorized:
						context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
						break;
					default:
						context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
						break;
				}
			}
		}

		/// <summary>
		///   Handle a http post request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected override async Task HandlePost(IRequestData requestData, HttpContext context)
		{
			if (!this.Validate(requestData)
			    || requestData.Query?.Any() == true
			    || string.IsNullOrWhiteSpace(requestData.Body))
			{
				context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			}
			else
			{
				var createResult = await this.service.Create(requestData.Body);
				switch (createResult.Result)
				{
					case CreateResult.Created:
						await this.ResultWithData(context, createResult.Entry, HttpStatusCode.Created);
						break;
					case CreateResult.AlreadyExists:
						context.Response.StatusCode = (int) HttpStatusCode.Conflict;
						break;
					case CreateResult.InvalidData:
						context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
						break;
					case CreateResult.Unauthorized:
						context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
						break;
					default:
						context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
						break;
				}
			}
		}

		/// <summary>
		///   Handle a http put request.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		protected override async Task HandlePut(IRequestData requestData, HttpContext context)
		{
			if (!this.Validate(requestData)
			    || requestData.Query?.Any() == true
			    || string.IsNullOrWhiteSpace(requestData.Body))
			{
				context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			}
			else
			{
				var updateResult = await this.service.Update(requestData.Body);
				switch (updateResult.Result)
				{
					case UpdateResult.Updated:
						await this.ResultWithData(context, updateResult.Entry, HttpStatusCode.OK);
						break;
					case UpdateResult.NotFound:
						context.Response.StatusCode = (int) HttpStatusCode.NotFound;
						break;
					case UpdateResult.InvalidData:
						context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
						break;
					case UpdateResult.Unauthorized:
						context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
						break;
					default:
						context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
						break;
				}
			}
		}

		/// <summary>
		///   Handle a http delete request that deletes all entries.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task HandleClear(IRequestData requestData, HttpContext context)
		{
			var clearResult = await this.service.Clear();
			switch (clearResult)
			{
				case ClearResult.Cleared:
					context.Response.StatusCode = (int) HttpStatusCode.NoContent;
					break;
				case ClearResult.Unauthorized:
					context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
					break;
				default:
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					break;
			}
		}

		/// <summary>
		///   Handle a http delete request that deletes a single entry.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task HandleDeleteEntry(IRequestData requestData, HttpContext context)
		{
			var entryId = (TEntryId) Convert.ChangeType(requestData.Query[Id], typeof(TEntryId));
			var deleteResult = await this.service.Delete(entryId);

			switch (deleteResult.Result)
			{
				case DeleteResult.Deleted:
					await this.ResultWithData(context, deleteResult.Entry, HttpStatusCode.OK);
					break;
				case DeleteResult.NotFound:
					context.Response.StatusCode = (int) HttpStatusCode.NotFound;
					break;
				case DeleteResult.InvalidData:
					context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
					break;
				case DeleteResult.Unauthorized:
					context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
					break;
				default:
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					break;
			}
		}

		/// <summary>
		///   Handle a http get request that reads a list of entries by its id.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task HandleGetEntries(IRequestData requestData, HttpContext context)
		{
			var entries = JsonConvert.DeserializeObject<Entries<TEntryId>>(requestData.Body);
			var readResultList = await this.service.Read(entries?.Ids);
			await this.ResultWithData(context, readResultList, HttpStatusCode.OK);
		}

		/// <summary>
		///   Handle a http get request that reads a single entry by its id.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task HandleGetEntry(IRequestData requestData, HttpContext context)
		{
			var entryId = (TEntryId) Convert.ChangeType(requestData.Query[Id], typeof(TEntryId));
			var readResult = await this.service.Read(entryId);
			switch (readResult.Result)
			{
				case ReadResult.Read:
					await this.ResultWithData(context, readResult.Entry, HttpStatusCode.OK);
					break;
				case ReadResult.NotFound:
					context.Response.StatusCode = (int) HttpStatusCode.NotFound;
					break;
				case ReadResult.InvalidData:
					context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
					break;
				case ReadResult.Unauthorized:
					context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
					break;
				default:
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					break;
			}
		}

		/// <summary>
		///   Handle a http get request that lists all entry ids.
		/// </summary>
		/// <param name="requestData">The collected request data from the <paramref name="context" />.</param>
		/// <param name="context">The http context of the function execution.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task HandleList(IRequestData requestData, HttpContext context)
		{
			var listResult = await this.service.List();
			switch (listResult.Result)
			{
				case ListResult.Completed:
					await this.ResultWithData(
						context,
						new Entries<TEntryId>
						{
							Ids = listResult.Entries.ToArray()
						},
						HttpStatusCode.OK);
					break;
				case ListResult.Unauthorized:
					context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
					break;
				default:
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					break;
			}
		}

		/// <summary>
		///   Sets the <paramref name="statusCode" /> to the <see cref="HttpContext.Response" /> and the <paramref name="data" />
		///   to the body of the response.
		/// </summary>
		/// <typeparam name="T">The type of the body data.</typeparam>
		/// <param name="context">The http context of the function execution.</param>
		/// <param name="data">The data that is added to the body.</param>
		/// <param name="statusCode">The http status code of the response.</param>
		/// <returns>A <see cref="Task" />.</returns>
		private async Task ResultWithData<T>(HttpContext context, T data, HttpStatusCode statusCode)
		{
			context.Response.StatusCode = (int) statusCode;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(JsonConvert.SerializeObject(data));
		}

		/// <summary>
		///   Validates the request data from the http request.
		/// </summary>
		/// <param name="requestData">The collected request data.</param>
		/// <returns>True if the data is valid and query/body data is valid.</returns>
		private bool Validate(IRequestData requestData)
		{
			// reject if requestData is null
			// reject if body and query is set
			if (requestData == null || !string.IsNullOrWhiteSpace(requestData.Body) && requestData.Query?.Any() == true)
			{
				return false;
			}

			// reject if more than one query item
			// reject if existing query item does not equal ID
			if (requestData.Query != null
			    && (requestData.Query.Count > 1 || requestData.Query.Count == 1 && !requestData.Query.ContainsKey(Id)))
			{
				return false;
			}

			return true;
		}
	}
}