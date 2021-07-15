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
		public async void ClearShouldReturnFalseIfDatabaseServiceReturnsFalse()
		{
			var stockService = new StockService(new DatabaseServiceMock(false));
			Assert.False(await stockService.Clear());
		}

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
		///   <see cref="StockService.Delete" /> should fail if database operation fails.
		/// </summary>
		[Fact]
		public async void DeleteShouldFailUnknownStockItemId()
		{
			var stockService = new StockService(new DatabaseServiceMock(false));
			Assert.False(await stockService.Delete(new Guid()));
		}

		/// <summary>
		///   <see cref="StockService.Delete" /> should succeed if database operation succeeds.
		/// </summary>
		[Fact]
		public async void DeleteShouldSucceedForKnownStockItemId()
		{
			var stockService = new StockService(new DatabaseServiceMock(true));
			Assert.True(await stockService.Delete(new Guid()));
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
		///   <see cref="StockService.Update" /> should fail if requesting more than available.
		/// </summary>
		[Fact]
		public async void UpdateShouldIndicateFailureIfRequestingMoreThanAvailable()
		{
			var databaseService = new DatabaseServiceMock(true);
			var stockService = new StockService(databaseService);
			var id = Guid.NewGuid();
			const int delta = -10;
			var (stockItem, isUpdated) = await stockService.Update(id, delta);
			Assert.NotNull(stockItem);
			Assert.False(isUpdated);
			Assert.Equal(2, databaseService.UpdateCallCount);
		}

		/// <summary>
		///   <see cref="StockService.Update" /> should fail if requested item does not exist.
		/// </summary>
		[Fact]
		public async void UpdateShouldIndicateNotFoundIfDatabaseServiceReturnsNull()
		{
			var databaseService = new DatabaseServiceMock(false);
			var stockService = new StockService(databaseService);
			var id = Guid.NewGuid();
			const int delta = 10;
			var (stockItem, _) = await stockService.Update(id, delta);
			Assert.Null(stockItem);
			Assert.Equal(1, databaseService.UpdateCallCount);
		}

		/// <summary>
		///   <see cref="StockService.Update" /> should succeed if requested item exists and delta is zero.
		/// </summary>
		[Fact]
		public async void UpdateShouldIndicateSuccessIfDatabaseServiceReturnsNullForDeltaZero()
		{
			var databaseService = new DatabaseServiceMock(true);
			var stockService = new StockService(databaseService);
			var id = Guid.NewGuid();
			const int delta = 0;
			var (stockItem, isUpdated) = await stockService.Update(id, delta);
			Assert.NotNull(stockItem);
			Assert.True(isUpdated);
			Assert.Equal(1, databaseService.UpdateCallCount);
		}

		/// <summary>
		///   <see cref="StockService.Update" /> should succeed if requested item exists and enough items in stock.
		/// </summary>
		[Fact]
		public async void UpdateShouldIndicateSuccessIfItemExistsAndEnoughItemsInStock()
		{
			var databaseService = new DatabaseServiceMock(true);
			var stockService = new StockService(databaseService);
			var id = Guid.NewGuid();
			const int delta = 10;
			var (stockItem, isUpdated) = await stockService.Update(id, delta);
			Assert.NotNull(stockItem);
			Assert.True(isUpdated);
			Assert.Equal(1, databaseService.UpdateCallCount);
		}
	}
}