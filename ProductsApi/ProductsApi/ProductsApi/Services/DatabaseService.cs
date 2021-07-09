namespace ProductsApi.Services
{
	using ProductsApi.Contracts;

	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides database operations for objects of type <see cref="IProduct"/>.
	/// </summary>
	public class DatabaseService : IDatabaseService
	{
		private static IDictionary<Guid, IProduct> database = new Dictionary<Guid, IProduct>();

		/// <summary>
		/// Creates a new product in the database.
		/// </summary>
		/// <param name="product">The product to be created.</param>
		/// <returns>True if the product is created, false if an entry with <see cref="IProduct.Id"/> already exists.</returns>
		public async Task<bool> Create(IProduct product)
		{
			if (database.ContainsKey(product.Id))
			{
				return false;
			}

			database[product.Id] = product;
			return true;
		}

		/// <summary>
		/// Delete a product from the database.
		/// </summary>
		/// <param name="id">The id of the product to be deleted.</param>
		/// <returns>True if the product is deleted, false if no product with the given <paramref name="id"/> exists.</returns>
		public async Task<bool> Delete(Guid id)
		{
			if (database.ContainsKey(id))
			{
				database.Remove(id);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Retrieve a list of all products in the database.
		/// </summary>
		/// <returns>A list of all products.</returns>
		public async Task<IEnumerable<IProduct>> List()
		{
			return database.Values;
		}

		/// <summary>
		/// Reads a product by the given <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The id of the product.</param>
		/// <returns>The product or null if no product with <paramref name="id"/> exists.</returns>
		public async Task<IProduct> ReadById(Guid id)
		{
			if (database.ContainsKey(id))
			{
				return database[id];
			}

			return null;
		}

		/// <summary>
		/// Updates an existing product.
		/// </summary>
		/// <param name="product">The new values for the product.</param>
		/// <returns>True if the product is updated and false if no product with <see cref="IProduct.Id"/> exists.</returns>
		public async Task<bool> Update(IProduct product)
		{
			if (database.ContainsKey(product.Id))
			{
				database[product.Id] = product;
				return true;
			}

			return false;
		}
	}
}
