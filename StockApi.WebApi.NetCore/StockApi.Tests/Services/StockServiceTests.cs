namespace StockApi.Tests.Services
{
	using System;
	using StockApi.Models;
	using StockApi.Services;
	using StockApi.Tests.Mocks;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="StockService" />.
	/// </summary>
	public class StockServiceTests
	{
		/// <summary>
		///   <see cref="StockService.Clear" /> should return true if <see cref="DatabaseServiceMock.Clear" /> returns true.
		/// </summary>
		[Fact]
		public async void ClearShouldReturnTrueIfDatabaseServiceReturnsTrue()
		{
			var stockService = new StockService(new DatabaseServiceMock(true));
			Assert.True(await stockService.Clear());
		}

		/// <summary>
		///   <see cref="StockService.Clear" /> should return true if <see cref="DatabaseServiceMock.Clear" /> returns true.
		/// </summary>
		[Fact]
		public async void ClearShouldReturnFalseIfDatabaseServiceReturnsFalse()
		{
			var stockService = new StockService(new DatabaseServiceMock(false));
			Assert.False(await stockService.Clear());
		}

		/// <summary>
		///   <see cref="StockService.Create" /> should return true if <see cref="DatabaseServiceMock.Create" /> returns true.
		/// </summary>
		[Fact]
		public async void CreateShouldReturnTrueIfDatabaseServiceReturnsTrue()
		{
			var stockService = new StockService(new DatabaseServiceMock(true));
			var stockItem = new StockItemDto {Id = new Guid(), InStock = 100};
			Assert.True(await stockService.Create(stockItem));
		}

		/// <summary>
		///   <see cref="StockService.Create" /> should return false if <see cref="DatabaseServiceMock.Create" /> returns false.
		/// </summary>
		[Fact]
		public async void CreateShouldReturnFalseIfDatabaseServiceReturnsFalse()
		{
			var stockService = new StockService(new DatabaseServiceMock(false));
			var stockItem = new StockItemDto {Id = new Guid(), InStock = 100};
			Assert.False(await stockService.Create(stockItem));
		}

		/// <summary>
		///   <see cref="StockService.ReadById" /> should return <see cref="StockItem" /> if item with given id exists.
		/// </summary>
		[Fact]
		public async void ReadByIdShouldReturnStockItemIfEntryIsFound()
		{
			var stockService = new StockService(new DatabaseServiceMock(true));
			var id = Guid.NewGuid();
			var stockItem = await stockService.ReadById(id);
			Assert.NotNull(stockItem);
			Assert.Equal(id, stockItem.Id);
		}

		/// <summary>
		///   <see cref="StockService.ReadById" /> should return null if no item with given id exists.
		/// </summary>
		[Fact]
		public async void ReadByIdShouldReturnNullIfNoEntryIsFound()
		{
			var stockService = new StockService(new DatabaseServiceMock(false));
			var id = Guid.NewGuid();
			var stockItem = await stockService.ReadById(id);
			Assert.Null(stockItem);
		}
	}
}