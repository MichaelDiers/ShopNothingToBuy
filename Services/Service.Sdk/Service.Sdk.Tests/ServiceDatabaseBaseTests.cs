namespace Service.Sdk.Tests
{
	using System;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Mocks;
	using Service.Sdk.Tests.Models;
	using Xunit;

	public class ServiceDatabaseBaseTests
	{
		[Fact]
		public async void ClearEntries_ShouldSucceed()
		{
			var service = InitService(true);
			var result = await service.Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Fact]
		public async void CreateEntry_ShouldFail()
		{
			var service = InitService(false);
			var result = await service.Create(new CreateEntry(10));
			Assert.Equal(CreateResult.AlreadyExists, result.Result);
		}

		[Fact]
		public async void CreateEntry_ShouldFailWithException()
		{
			var service = InitService(false, true);
			var result = await service.Create(new CreateEntry(10));
			Assert.Equal(CreateResult.InternalError, result.Result);
		}

		[Fact]
		public async void CreateEntry_ShouldSucceed()
		{
			var service = InitService(true);
			var result = await service.Create(new CreateEntry(10));
			Assert.Equal(CreateResult.Created, result.Result);
		}

		[Fact]
		public async void DeleteEntry_ShouldSucceed()
		{
			var service = InitService(true);
			var result = await service.Delete(Guid.NewGuid().ToString());
			Assert.Equal(DeleteResult.Deleted, result.Result);
		}

		[Fact]
		public async void ExistsEntry_ShouldSucceed()
		{
			var service = InitService(true);
			var result = await service.Exists(Guid.NewGuid().ToString());
			Assert.Equal(ExistsResult.Exists, result);
		}

		[Fact]
		public async void ListEntries_ShouldSucceed()
		{
			var service = InitService(true);
			var result = await service.List();
			Assert.Equal(ListResult.Completed, result.Result);
		}

		[Fact]
		public async void ReadEntry_ShouldSucceed()
		{
			var service = InitService(true);
			var result = await service.Read(Guid.NewGuid().ToString());
			Assert.Equal(ReadResult.Read, result.Result);
		}

		[Fact]
		public async void UpdateEntry_ShouldFail()
		{
			var service = InitService(false);
			var result = await service.Update(new UpdateEntry(Guid.NewGuid().ToString(), "my new value"));
			Assert.Equal(UpdateResult.NotFound, result.Result);
		}

		[Fact]
		public async void UpdateEntry_ShouldFailWithException()
		{
			var service = InitService(false, true);
			var result = await service.Update(new UpdateEntry(Guid.NewGuid().ToString(), "my new value"));
			Assert.Equal(UpdateResult.InternalError, result.Result);
		}

		[Fact]
		public async void UpdateEntry_ShouldSucceed()
		{
			var service = InitService(true);
			var result = await service.Update(new UpdateEntry(Guid.NewGuid().ToString(), "my new value"));
			Assert.Equal(UpdateResult.Updated, result.Result);
		}

		private static ServiceDatabaseBaseMock InitService(bool succeed, bool throwException = false)
		{
			var logger = new LoggerMock();
			return new ServiceDatabaseBaseMock(
				logger,
				new ValidatorMock(),
				new DatabaseServiceMock(logger, new DefaultValidator<StringEntry, StringEntry, string>(), true),
				succeed,
				throwException);
		}
	}
}