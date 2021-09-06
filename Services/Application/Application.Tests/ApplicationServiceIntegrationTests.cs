namespace Application.Tests
{
	using Application.Contracts;
	using Application.Services;
	using Application.Services.Models;
	using Application.Tests.Models;
	using Microsoft.Extensions.Configuration;
	using MongoDatabase.Models;
	using Service.Sdk.Contracts;
	using Xunit;

	public class ApplicationServiceIntegrationTests
	{
		private const bool Skip = true;

		private static IApplicationService service;

		[Fact]
		public async void Clear_ShouldSucceed()
		{
			if (Skip)
			{
				return;
			}

			var result = await InitService().Clear();
			Assert.Equal(ClearResult.Cleared, result);

			var listResult = await InitService().List();
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
			var result = await InitService().Create(
				new CreateApplicationEntry
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
			var createResult = await InitService().Create(
				new CreateApplicationEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var deleteResult = await InitService().Delete(createResult.Entry.Id);

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

			var createResult = await InitService().Create(
				new CreateApplicationEntry
				{
					Name = "name"
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var existsResult = await InitService().Exists(createResult.Entry.Id);
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
			var createResult = await InitService().Create(
				new CreateApplicationEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var listResult = await InitService().List();

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
			var createResult = await InitService().Create(
				new CreateApplicationEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			var readResult = await InitService().Read(createResult.Entry.Id);

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
			var createResult = await InitService().Create(
				new CreateApplicationEntry
				{
					Name = name
				});
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.NotNull(createResult.Entry);
			Assert.NotNull(createResult.Entry.Id);

			const string newName = name + "2";
			var updateResult = await InitService().Update(
				new UpdateApplicationEntry
				{
					Id = createResult.Entry.Id,
					Name = newName
				});

			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(createResult.Entry.Id, updateResult.Entry.Id);
			Assert.Equal(newName, updateResult.Entry.Name);
		}

		private static IApplicationService InitService()
		{
			if (service != null)
			{
				return service;
			}

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.test.json", false)
				.Build();

			var mongoDbConfiguration = config.GetSection("MongoDb").Get<MongoDbConfiguration>();
			service = new ApplicationService(new LoggerMock(), mongoDbConfiguration);

			return service;
		}
	}
}