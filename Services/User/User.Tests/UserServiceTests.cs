namespace User.Tests
{
	using System;
	using Service.Sdk.Contracts;
	using User.Services;
	using User.Services.Models;
	using User.Tests.Mocks;
	using Xunit;

	public class UserServiceTests
	{
		[Fact]
		public async void Clear_ShouldSucceed()
		{
			var result = await new UserService(new LoggerMock()).Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Fact]
		public async void Create_ShouldSucceed()
		{
			var createUser = new CreateUserEntry();
			var result = await new UserService(new LoggerMock()).Create(createUser);
			Assert.Equal(CreateResult.Created, result.Result);
			Assert.NotNull(result.Entry);
			Assert.True(
				!string.IsNullOrWhiteSpace(result.Entry.Id)
				&& Guid.TryParse(result.Entry.Id, out var guid)
				&& guid != Guid.Empty);
		}

		[Fact]
		public async void Delete_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var result = await new UserService(new LoggerMock()).Delete(id);
			Assert.Equal(DeleteResult.Deleted, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void Read_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var result = await new UserService(new LoggerMock()).Read(id);
			Assert.Equal(ReadResult.Read, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void Update_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var updateEntry = new UpdateUserEntry
			{
				Id = id
			};

			var result = await new UserService(new LoggerMock()).Update(updateEntry);
			Assert.Equal(UpdateResult.Updated, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void WorkflowTest()
		{
			var service = new UserService(new LoggerMock());

			var createUser = new CreateUserEntry();
			var createResult = await service.Create(createUser);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var updateUser = new UpdateUserEntry
			{
				Id = createResult.Entry.Id
			};
			var updateResult = await service.Update(updateUser);
			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(createResult.Entry.Id, updateResult.Entry.Id);

			var deleteResult = await service.Delete(createResult.Entry.Id);
			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(createResult.Entry.Id, deleteResult.Entry.Id);
		}
	}
}