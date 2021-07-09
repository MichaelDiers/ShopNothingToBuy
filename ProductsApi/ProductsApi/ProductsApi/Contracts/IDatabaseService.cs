namespace ProductsApi.Contracts
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IDatabaseService
	{
		Task<IEnumerable<IProduct>> List();
	}
}
