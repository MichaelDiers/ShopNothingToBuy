namespace User.Tests
{
	using System;
	using Service.Sdk.Contracts;
	using User.Contracts;
	using User.Services;
	using User.Services.Models;
	using User.Tests.Mocks;
	using Xunit;

	public class UserServiceTests
	{
		[Fact]
		public async void Clear_ShouldSucceed()
		{
			var service = InitUserService();
			var result = await service.Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Fact]
		public async void Create_ShouldSucceed()
		{
			var createUser = new CreateUserEntry
			{
				Name = Guid.NewGuid().ToString()
			};

			var service = InitUserService();
			var result = await service.Create(createUser);
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
			var service = InitUserService();
			var result = await service.Delete(id);
			Assert.Equal(DeleteResult.Deleted, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void Exists_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var service = InitUserService();
			var result = await service.Exists(id);
			Assert.Equal(ExistsResult.Exists, result);
		}

		[Fact]
		public async void List_ShouldSucceed()
		{
			var service = InitUserService();
			var result = await service.List();
			Assert.Equal(ListResult.Completed, result.Result);
			Assert.NotNull(result.Entries);
			Assert.Empty(result.Entries);
		}

		[Fact]
		public async void Read_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var service = InitUserService();
			var result = await service.Read(id);
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
				Id = id,
				Name = "name"
			};

			var service = InitUserService();
			var result = await service.Update(updateEntry);
			Assert.Equal(UpdateResult.Updated, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void WorkflowTest()
		{
			var service = InitUserService();

			var createUser = new CreateUserEntry
			{
				Name = "Name"
			};
			var createResult = await service.Create(createUser);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var updateUser = new UpdateUserEntry
			{
				Id = createResult.Entry.Id,
				Name = createResult.Entry.Name + "2"
			};
			var updateResult = await service.Update(updateUser);
			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(createResult.Entry.Id, updateResult.Entry.Id);

			var deleteResult = await service.Delete(createResult.Entry.Id);
			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(createResult.Entry.Id, deleteResult.Entry.Id);
		}

		private static IUserService InitUserService()
		{
			return new UserService(new LoggerMock(), new UserServiceValidator(), new DatabaseServiceMock());
		}
	}
}