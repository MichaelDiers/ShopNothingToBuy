namespace OrdersApi.Server.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;
	using MongoDB.Bson;
	using MongoDB.Driver;
	using OrdersApi.Server.Contracts;
	using OrdersApi.Server.Models;

	/// <summary>
	///   Provides mongodb database operations.
	/// </summary>
	public class DatabaseService : IDatabaseService
	{
		/// <summary>
		///   Name of the amount in the mongodb.
		/// </summary>
		private const string DatabaseAmount = "amount";

		/// <summary>
		///   Name of customer id in mongodb.
		/// </summary>
		private const string DatabaseCustomerId = "customerId";

		/// <summary>
		///   Name of the order id in the mongodb.
		/// </summary>
		private const string DatabaseOrderId = "orderId";

		/// <summary>
		///   Name of the order status in the mongodb.
		/// </summary>
		private const string DatabaseOrderStatus = "status";

		/// <summary>
		///   Name of positions in the mongodb.
		/// </summary>
		private const string DatabasePositions = "positions";

		/// <summary>
		///   Name of the product id in the mongodb.
		/// </summary>
		private const string DatabaseProductId = "productId";

		/// <summary>
		///   The <see cref="MongoClient" /> for accessing the database.
		/// </summary>
		private readonly MongoClient client;

		/// <summary>
		///   The mongodb collection containing the orders.
		/// </summary>
		private readonly IMongoCollection<BsonDocument> collection;

		/// <summary>
		///   The name of the collection containing orders.
		/// </summary>
		private readonly string collectionName;

		/// <summary>
		///   The mongodb database containing the <see cref="collection" />.
		/// </summary>
		private readonly IMongoDatabase database;

		/// <summary>
		///   Create a new instance of <see cref="DatabaseService" />.
		/// </summary>
		/// <param name="configuration"></param>
		public DatabaseService(IConfiguration configuration)
		{
			this.collectionName = configuration["CollectionName"];

			var settings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString("MongoDb"));
			this.client = new MongoClient(settings);
			this.database = this.client.GetDatabase(configuration["DatabaseName"]);
			this.collection = this.database.GetCollection<BsonDocument>(this.collectionName);
		}

		/// <summary>
		///   Delete the complete collection of storage entries.
		/// </summary>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task Clear()
		{
			await this.database.DropCollectionAsync(this.collectionName);
		}

		/// <summary>
		///   Create a new order in the database.
		/// </summary>
		/// <param name="order">The order to be created.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task Create(Order order)
		{
			var dbPositions = new BsonArray(
				order.Positions.Select(
					position => new BsonDocument
					{
						{DatabaseProductId, position.ProductId.ToString()},
						{DatabaseAmount, new BsonInt32((int) position.Amount)}
					}));
			var document = new BsonDocument
			{
				{DatabaseOrderId, order.Id.ToString()},
				{DatabasePositions, dbPositions},
				{DatabaseOrderStatus, (int) order.Status},
				{DatabaseCustomerId, order.CustomerId.ToString()}
			};

			await this.collection.InsertOneAsync(document);
		}

		/// <summary>
		///   Delete an order of a customer by its id.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		public async Task<bool> DeleteOrder(Guid customerId, Guid orderId)
		{
			var filterBuilder = Builders<BsonDocument>.Filter;
			var filter = filterBuilder.And(
				filterBuilder.Eq(DatabaseOrderId, orderId.ToString()),
				filterBuilder.Eq(DatabaseCustomerId, customerId.ToString()));
			var deleterResult = await this.collection.DeleteOneAsync(filter);
			return deleterResult.IsAcknowledged && deleterResult.DeletedCount == 1;
		}

		/// <summary>
		///   List all known ids of orders.
		/// </summary>
		/// <returns>A list of order ids.</returns>
		public async Task<IEnumerable<Guid>> ListOrderIds()
		{
			var ids = new List<Guid>();

			var cursor = await this.collection.FindAsync(new BsonDocument());
			while (await cursor.MoveNextAsync())
			{
				ids.AddRange(cursor.Current.Select(document => Guid.Parse(document[DatabaseOrderId].AsString)));
			}

			return ids;
		}

		/// <summary>
		///   List all known ids of orders for given customer id.
		/// </summary>
		/// <param name="customerId">The id of the customer id.</param>
		/// <returns>A list of order ids.</returns>
		public async Task<IEnumerable<Guid>> ListOrderIds(Guid customerId)
		{
			var ids = new List<Guid>();

			var filterBuilder = Builders<BsonDocument>.Filter;
			var filter = filterBuilder.Eq(DatabaseCustomerId, customerId.ToString());
			var cursor = await this.collection.FindAsync(filter);
			while (await cursor.MoveNextAsync())
			{
				ids.AddRange(cursor.Current.Select(document => Guid.Parse(document[DatabaseOrderId].AsString)));
			}

			return ids;
		}

		/// <summary>
		///   Read an order by its id for a specified customer.
		/// </summary>
		/// <param name="customerId">The id of the customer.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <returns>
		///   An instance of <see cref="Order" /> or null if no matching order is found.
		/// </returns>
		public async Task<Order> ReadOrder(Guid customerId, Guid orderId)
		{
			var filterBuilder = Builders<BsonDocument>.Filter;
			var filter =
				filterBuilder.And(
					filterBuilder.Eq(DatabaseOrderId, orderId.ToString()),
					filterBuilder.Eq(
						DatabaseCustomerId,
						customerId.ToString()));

			var cursor = await this.collection.FindAsync(
				filter,
				new FindOptions<BsonDocument>
				{
					Limit = 1
				});
			if (await cursor.MoveNextAsync())
			{
				var document = cursor.Current.FirstOrDefault();
				if (document != null)
				{
					var order = new Order
					{
						CustomerId = Guid.Parse(document[DatabaseCustomerId].AsString),
						Id = Guid.Parse(document[DatabaseOrderId].AsString),
						Positions = document[DatabasePositions].AsBsonArray.Select(
							entry => new Position
							{
								Amount = (uint) entry[DatabaseAmount].AsInt32,
								ProductId = Guid.Parse(entry[DatabaseProductId].AsString)
							}).ToArray(),
						Status = (OrderStatus) document[DatabaseOrderStatus].AsInt32
					};
					return order;
				}
			}

			return null;
		}

		/// <summary>
		///   Update the order status for an order of a given customer to a new status.
		/// </summary>
		/// <param name="customerId">The customer that owns the order.</param>
		/// <param name="orderId">The id of the order.</param>
		/// <param name="newOrderStatus">The new status of the order.</param>
		/// <returns>True if the operation succeeds and false otherwise.</returns>
		public async Task<bool> UpdateOrderStatus(Guid customerId, Guid orderId, OrderStatus newOrderStatus)
		{
			var filterBuilder = Builders<BsonDocument>.Filter;
			var filter =
				filterBuilder.And(
					filterBuilder.Eq(DatabaseOrderId, orderId.ToString()),
					filterBuilder.Eq(
						DatabaseCustomerId,
						customerId.ToString()));
			var update = Builders<BsonDocument>.Update.Set(DatabaseOrderStatus, (int) newOrderStatus);
			var response = await this.collection.UpdateOneAsync(filter, update);
			return response.IsAcknowledged && response.ModifiedCount == 1;
		}
	}
}