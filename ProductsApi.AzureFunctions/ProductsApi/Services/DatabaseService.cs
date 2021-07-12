namespace ProductsApi.Services
{
	using Microsoft.Azure.Cosmos;
	using Microsoft.Azure.Cosmos.Linq;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;

	using ProductsApi.Contracts;
	using ProductsApi.Extensions;
	using ProductsApi.Models;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides database operations for objects of type <see cref="Product"/>.
	/// </summary>
	public class DatabaseService : IDatabaseService
	{
		/// <summary>
		/// Used to access the cosmos db.
		/// </summary>
		private readonly CosmosClient cosmosClient;

		/// <summary>
		/// The name of the cosmos container in that items are stored.
		/// </summary>
		private readonly string containerName;

		/// <summary>
		/// The cosmos database instance.
		/// </summary>
		private readonly Database database;

		/// <summary>
		/// The container in that items are stored.
		/// </summary>
		private Container container;

		/// <summary>
		/// Create a new instance of <see cref="DatabaseService"/>.
		/// </summary>
		/// <param name="cosmosClient">The client used for accessing the cosmos db.</param>
		/// <param name="configuration">Used to access the configuration of funtions.</param>
		public DatabaseService(CosmosClient cosmosClient, IConfiguration configuration)
		{
			if (configuration is null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			this.cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
			this.database = this.cosmosClient.GetDatabase(configuration.GetValue<string>("DatabaseName"));
			this.containerName = configuration.GetValue<string>("ContainerName");
			this.container = this.database.GetContainer(this.containerName);
		}

		/// <summary>
		/// Delete all items from the database container.
		/// </summary>
		/// <param name="log">/// <param name="log">An <see cref="ILogger"/> instance.</param></param>
		/// <returns>True if all entries are deleted, otherwise false.</returns>/// <summary>
		/// Delete all items from the database container.
		/// </summary>
		/// <param name="log">/// <param name="log">An <see cref="ILogger"/> instance.</param></param>
		/// <returns>True if all entries are deleted, otherwise false.</returns>
		public async Task<bool> Clear(ILogger log)
		{
			try
			{
				// delete the container instead of single items
				var _ = await this.container.DeleteContainerAsync();

				// recreate the container
				var containerProperties = new ContainerProperties(this.containerName, "/id");
				var response = await this.database.CreateContainerAsync(containerProperties);
				this.container = response.Container;

				return true;
			}
			catch (CosmosException ex)
			{
				HandleCosmosException(ex, log);
			}

			return false;
		}

		/// <summary>
		/// Creates a new product in the database.
		/// </summary>
		/// <param name="product">The product to be created.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>True if the product is created, false if an entry with <see cref="Product.Id"/> already exists.</returns>
		public async Task<bool> Create(Product product, ILogger log)
		{
			try
			{
				var createdProduct = await container.CreateItemAsync(product);
				return true;
			}
			catch (CosmosException ex)
			{
				HandleCosmosException(ex, log);
				return false;
			}
		}		

		/// <summary>
		/// Delete a product from the database.
		/// </summary>
		/// <param name="id">The id of the product to be deleted.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>True if the product is deleted, false if no product with the given <paramref name="id"/> exists.</returns>
		public async Task<bool> Delete(Guid id, ILogger log)
		{
			try
			{
				var _ = await container.DeleteItemAsync<Product>(id.ToString(), new PartitionKey(id.ToString()));
				return true;
			}
			catch (CosmosException ex)
			{
				HandleCosmosException(ex, log, HttpStatusCode.NotFound);
			}

			return false;
		}

		/// <summary>
		/// Retrieve a list of all products in the database.
		/// </summary>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <param name="validProductsOnly">True specifies that only <see cref="Product"/> objects are 
		///		loaded that satisfy the formal json definition of the <see cref="Product"/>, otherwise
		///		all products are loaded - even if not json serializable.
		/// </param>
		/// <returns>A list of all products.</returns>
		public async Task<IEnumerable<Product>> List(ILogger log, bool validProductsOnly = true)
		{
			var products = new List<Product>();
			try
			{
				var iterator = container.GetItemLinqQueryable<Product>().ToFeedIterator();
				while (iterator.HasMoreResults)
				{
					var next = await iterator.ReadNextAsync();
					products.AddRange(next.ToArray().Where((product) => product.IsValid(validProductsOnly)));
				}
			}
			catch (CosmosException ex)
			{
				HandleCosmosException(ex, log);
			}

			return products;
		}

		/// <summary>
		/// Reads a product by the given <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The id of the product.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>The product or null if no product with <paramref name="id"/> exists.</returns>
		public async Task<Product> ReadById(Guid id, ILogger log)
		{
			try
			{
				var product = await container.ReadItemAsync<Product>(id.ToString(), new PartitionKey(id.ToString()));
				return product;
			}
			catch (CosmosException ex)
			{
				HandleCosmosException(ex, log, HttpStatusCode.NotFound);
				return null;
			}
		}

		/// <summary>
		/// Updates an existing product.
		/// </summary>
		/// <param name="product">The new values for the product.</param>
		/// <param name="log">An <see cref="ILogger"/> instance.</param>
		/// <returns>True if the product is updated and false if no product with <see cref="Product.Id"/> exists.</returns>
		public async Task<bool> Update(Product product, ILogger log)
		{
			try
			{
				var databaseProduct = await ReadById(product.Id, log);
				if (databaseProduct != null && databaseProduct.Id == product.Id)
				{
					var _ = await container.ReplaceItemAsync<Product>(product, product.Id.ToString());
					return true;
				}
			}
			catch (CosmosException ex)
			{
				HandleCosmosException(ex, log, HttpStatusCode.NotFound);
			}

			return false;
		}

		/// <summary>
		/// Log the given database error to the error log if the status code is not excluded.
		/// </summary>
		/// <param name="ex">The cosmos database exception.</param>
		/// <param name="log">The logger used for writing the error log.</param>
		/// <param name="codes">If the <see cref="CosmosException.StatusCode"/> is included in
		///		<paramref name="codes"/> the exception will be ignored.
		/// </param>
		private static void HandleCosmosException(CosmosException ex, ILogger log, params HttpStatusCode[] codes)
		{
			if (ex != null 
				&& log != null 
				&& (codes is null || codes.Length == 0 || codes.All((code) => code != ex.StatusCode)))
			{
				log.LogError(ex.ToString());
			}
		}
	}
}
