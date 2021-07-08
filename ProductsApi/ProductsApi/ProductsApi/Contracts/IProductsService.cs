namespace ProductsApi.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IProductsService
	{
		Task<IEnumerable<IProduct>> ReadProducts();
	}
}
