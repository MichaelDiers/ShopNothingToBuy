namespace Service.Sdk.Tests
{
	using System;
	using Service.Sdk.Contracts;
	using Service.Sdk.Tests.Mocks;
	using Service.Sdk.Tests.Models;
	using Xunit;

	public class DatabaseServiceTests
	{
		[Fact]
		public async void CreateEntry_ShouldFailIfEntryAlreadyExists()
		{
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(), true);
			var result = await service.Create(new StringEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
			Assert.Equal(CreateResult.AlreadyExists, result.Result);
			Assert.Null(result.Entry);
		}

		[Fact]
		public async void CreateEntry_ShouldFailIfEntryIdIsInvalid()
		{
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(true, false, true), false);
			var result = await service.Create(new StringEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
			Assert.Equal(CreateResult.InvalidData, result.Result);
			Assert.Null(result.Entry);
		}

		[Fact]
		public async void CreateEntry_ShouldSucceed()
		{
			var entry = new StringEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(), false);
			var result = await service.Create(entry);
			Assert.Equal(CreateResult.Created, result.Result);
			Assert.Equal(entry.Id, result.Entry.Id);
			Assert.Equal(entry.Value, result.Entry.Value);
		}

		[Fact]
		public async void DeleteEntry_ShouldFailIfEntryDoesNotExists()
		{
			var id = Guid.NewGuid().ToString();
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(), false);
			var result = await service.Delete(id);
			Assert.Equal(DeleteResult.NotFound, result.Result);
			Assert.Null(result.Entry);
		}

		[Fact]
		public async void DeleteEntry_ShouldFailIfEntryIdIsInvalid()
		{
			var id = Guid.NewGuid().ToString();
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(true, false, true), true);
			var result = await service.Delete(id);
			Assert.Equal(DeleteResult.InvalidData, result.Result);
			Assert.Null(result.Entry);
		}

		[Fact]
		public async void DeleteEntry_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(), true);
			var result = await service.Delete(id);
			Assert.Equal(DeleteResult.Deleted, result.Result);
			Assert.Equal(id, result.Entry.Id);
			Assert.NotNull(result.Entry.Value);
		}

		[Fact]
		public async void UpdateEntry_ShouldFailIfEntryDoesNotExists()
		{
			var entry = new StringEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(), false);
			var result = await service.Update(entry);
			Assert.Equal(UpdateResult.NotFound, result.Result);
			Assert.Null(result.Entry);
		}

		[Fact]
		public async void UpdateEntry_ShouldFailIfEntryIdIsInvalid()
		{
			var entry = new StringEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(true, false, true), true);
			var result = await service.Update(entry);
			Assert.Equal(UpdateResult.InvalidData, result.Result);
			Assert.Null(result.Entry);
		}

		[Fact]
		public async void UpdateEntry_ShouldSucceed()
		{
			var entry = new StringEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
			var service = new DatabaseServiceMock(new LoggerMock(), new DatabaseValidatorMock(), true);
			var result = await service.Update(entry);
			Assert.Equal(UpdateResult.Updated, result.Result);
			Assert.Equal(entry.Id, result.Entry.Id);
			Assert.Equal(entry.Value, result.Entry.Value);
		}
	}
}