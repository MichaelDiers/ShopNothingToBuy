namespace ProductsApi.Services
{
	using ProductsApi.Contracts;
	using ProductsApi.Models;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class DatabaseService : IDatabaseService
	{
		public async Task<IEnumerable<IProduct>> List()
		{
			return new[] { new Product { Description = "foo", Id = Guid.NewGuid(), Name = "bar" } };
		}
	}
}
