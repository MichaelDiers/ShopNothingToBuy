namespace StockApi.Services
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;
	using StackExchange.Redis;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   Handles database operations on redis database.
	/// </summary>
	public class DatabaseService : IDatabaseService, IDisposable
	{
		/// <summary>
		///   Redis database connection.
		/// </summary>
		private readonly IConnectionMultiplexer redis;

		/// <summary>
		///   Creates a new instance of <see cref="DatabaseService" />.
		/// </summary>
		/// <param name="configuration">Provides access to configuration data.</param>
		public DatabaseService(IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			var connectionsString = configuration["RedisConnectionString"];
			this.redis = ConnectionMultiplexer.Connect(connectionsString);
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
				// Todo: logging
				Console.WriteLine(ex);
				return false;
			}
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