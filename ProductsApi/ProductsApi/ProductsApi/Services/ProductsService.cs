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

		public async Task<IEnumerable<IProductDTO>> ReadProducts()
		{
			var products = await this.databaseService.Read();
			return products.Select(product => product.ToDTO());
		}
	}
}
