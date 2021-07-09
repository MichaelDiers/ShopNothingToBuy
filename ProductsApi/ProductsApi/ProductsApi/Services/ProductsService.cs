namespace ProductsApi.Services
{
	using ProductsApi.Contracts;
	using ProductsApi.Extensions;

	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class ProductsService : IProductsService
	{
		private readonly IDatabaseService databaseService;

		public ProductsService(IDatabaseService databaseService)
		{
			this.databaseService = databaseService;
		}

		public async Task<IEnumerable<IProductDTO>> ListProducts()
		{
			var products = await this.databaseService.List();
			return products.Select(product => product.ToDTO());
		}
	}
}
