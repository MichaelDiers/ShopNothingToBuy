namespace StockApi.Tests.Mocks
{
	using System.Threading.Tasks;
	using StockApi.Contracts;
	using StockApi.Models;

	public class DatabaseServiceMock : IDatabaseService
	{
		private readonly bool defaultReturnValue;

		public DatabaseServiceMock(bool defaultReturnValue)
		{
			this.defaultReturnValue = defaultReturnValue;
		}

		public Task<bool> Create(StockItem stockItem)
		{
			return Task.FromResult(this.defaultReturnValue);
		}
	}
}