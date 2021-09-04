﻿namespace User.Tests
{
	using Microsoft.Extensions.Configuration;
	using MongoDatabase.Models;
	using Service.Sdk.Contracts;
	using User.Contracts;
	using User.Services;
	using User.Services.Models;
	using User.Tests.Mocks;
	using Xunit;

	public class UserServiceIntegrationTests
	{
		private const bool Skip = true;

		private static IUserService userService;

		[Fact]
		public async void Clear_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			var result = await InitUserService().Clear();
			Assert.Equal(ClearResult.Cleared, result);

			var listResult = await InitUserService().List();
			Assert.Equal(ListResult.Completed, listResult.Result);
			Assert.Empty(listResult.Entries);
		}

		[Fact]
		public async void Create_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			const string name = "my name";
			var result = await InitUserService().Create(
				new CreateUserEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, result.Result);
			Assert.NotNull(result.Entry);
			Assert.NotNull(result.Entry.Id);
			Assert.Equal(name, result.Entry.Name);
		}

		[Fact]
		public async void Delete_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			const string name = "my name delete";
			var createResult = await InitUserService().Create(
				new CreateUserEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var deleteResult = await InitUserService().Delete(createResult.Entry.Id);

			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(createResult.Entry.Id, deleteResult.Entry.Id);
			Assert.Equal(name, deleteResult.Entry.Name);
		}

		[Fact]
		public async void Exists_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			var createResult = await InitUserService().Create(
				new CreateUserEntry
				{
					Name = "name"
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var existsResult = await InitUserService().Exists(createResult.Entry.Id);
			Assert.Equal(ExistsResult.Exists, existsResult);
		}

		[Fact]
		public async void List_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			const string name = "my name list";
			var createResult = await InitUserService().Create(
				new CreateUserEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var listResult = await InitUserService().List();

			Assert.Equal(ListResult.Completed, listResult.Result);
			Assert.Contains(createResult.Entry.Id, listResult.Entries);
		}

		[Fact]
		public async void Read_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			const string name = "my name read";
			var createResult = await InitUserService().Create(
				new CreateUserEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var readResult = await InitUserService().Read(createResult.Entry.Id);

			Assert.Equal(ReadResult.Read, readResult.Result);
			Assert.Equal(createResult.Entry.Id, readResult.Entry.Id);
			Assert.Equal(name, readResult.Entry.Name);
		}

		[Fact]
		public async void Update_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			const string name = "my name update";
			var createResult = await InitUserService().Create(
				new CreateUserEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			const string newName = name + "2";
			var updateResult = await InitUserService().Update(
				new UpdateUserEntry
				{
					Id = createResult.Entry.Id,
					Name = newName
				});

			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(createResult.Entry.Id, updateResult.Entry.Id);
			Assert.Equal(newName, updateResult.Entry.Name);
		}

		private static IUserService InitUserService()
		{
			if (userService != null)
			{
				return userService;
			}

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.test.json", false)
				.Build();

			var mongoDbConfiguration = config.GetSection("MongoDb").Get<MongoDbConfiguration>();
			userService = new UserService(new LoggerMock(), mongoDbConfiguration);

			return userService;
		}
	}
}