namespace ProductsApi.Services
{
	using ProductsApi.Contracts;

	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class ProductsService : IProductsService
	{
		public async Task<IEnumerable<IProduct>> ReadProducts()
		{
			return Enumerable.Empty<IProduct>();
		}
	}
}
