namespace StockApi.Services
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;
	using StackExchange.Redis;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   Handles database operations on redis database.
	/// </summary>
	public class DatabaseService : IDatabaseService, IDisposable
	{
		/// <summary>
		///   Logger for errors.
		/// </summary>
		private readonly ILogger<DatabaseService> logger;

		/// <summary>
		///   Redis database connection.
		/// </summary>
		private readonly IConnectionMultiplexer redis;

		/// <summary>
		///   Creates a new instance of <see cref="DatabaseService" />.
		/// </summary>
		/// <param name="configuration">Provides access to configuration data.</param>
		/// <param name="logger">Used for error logging.</param>
		public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			var connectionsString = configuration["RedisConnectionString"];
			this.redis = ConnectionMultiplexer.Connect(connectionsString);
		}

		/// <summary>
		///   Clear all entries from database.
		/// </summary>
		/// <returns>True if operation succeeds and false otherwise.</returns>
		public async Task<bool> Clear()
		{
			try
			{
				var endPoints = this.redis.GetEndPoints();
				foreach (var endPoint in endPoints)
				{
					await this.redis.GetServer(endPoint).FlushDatabaseAsync();
				}

				return true;
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, "Error while clearing database.");
			}

			return false;
		}

		/// <summary>
		///   Create a new entry in the database.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItem" /> to be stored.</param>
		/// <returns>
		///   True if <see cref="StockItem" /> is stored and otherwise false. False indicates a database error or that the
		///   <see cref="StockItem.Id" /> already exists.
		/// </returns>
		public async Task<bool> Create(StockItem stockItem)
		{
			try
			{
				var database = this.redis.GetDatabase();
				if (await database.KeyExistsAsync(stockItem.Id.ToString()))
				{
					return false;
				}

				return await database.StringSetAsync(stockItem.Id.ToString(), stockItem.InStock);
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, "Error while creating stock item.");
				return false;
			}
		}

		/// <summary>
		///   Read a <see cref="StockItem" /> by its id.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <returns>A <see cref="StockItem" /> if an item with given id exists, null otherwise.</returns>
		public async Task<StockItem> ReadById(Guid id)
		{
			try
			{
				var database = this.redis.GetDatabase();
				var result = await database.StringGetAsync(id.ToString());
				if (!result.IsNullOrEmpty && uint.TryParse(result, out var inStock))
				{
					return new StockItem
					{
						Id = id, InStock = inStock
					};
				}
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, "Error while reading stock item.");
			}

			return null;
		}

		/// <summary>
		///   Dispose database.
		/// </summary>
		public void Dispose()
		{
			this.redis?.Dispose();
		}
	}
}