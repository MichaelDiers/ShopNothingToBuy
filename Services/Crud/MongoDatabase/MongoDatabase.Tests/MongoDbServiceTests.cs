namespace MongoDatabase.Tests
{
	using System;
	using Microsoft.Extensions.Configuration;
	using MongoDatabase.Services;
	using MongoDatabase.Tests.Mocks;
	using MongoDatabase.Tests.Models;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Contracts.Crud.Database;
	using Service.Sdk.Models.Crud.Database;
	using Service.Sdk.Tests.Mocks;
	using Xunit;

	public class MongoDbServiceTests
	{
		private const bool Skip = true;

		private static IDatabaseService<Entry, string> databaseService;

		[Fact]
		public async void Clear()
		{
			if (Skip)
			{
				return;
			}

			var id = Guid.NewGuid().ToString();
			const string value = "my new create value";
			var createResult = await InitDatabaseService().Create(new Entry(id, value));
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.Equal(id, createResult.Entry.Id);
			Assert.Equal(value, createResult.Entry.Value);

			var clearResult = await InitDatabaseService().Clear();
			Assert.Equal(ClearResult.Cleared, clearResult);

			var existsResult = await InitDatabaseService().Exists(id);
			Assert.Equal(ExistsResult.NotFound, existsResult);
		}

		[Fact]
		public async void Create()
		{
			if (Skip)
			{
				return;
			}

			var id = Guid.NewGuid().ToString();
			const string value = "my new create value";
			var result = await InitDatabaseService().Create(new Entry(id, value));
			Assert.Equal(CreateResult.Created, result.Result);
			Assert.Equal(id, result.Entry.Id);
			Assert.Equal(value, result.Entry.Value);
		}

		[Fact]
		public async void Delete()
		{
			if (Skip)
			{
				return;
			}

			var id = Guid.NewGuid().ToString();
			const string value = "my new create value";
			var createResult = await InitDatabaseService().Create(new Entry(id, value));
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.Equal(id, createResult.Entry.Id);
			Assert.Equal(value, createResult.Entry.Value);

			var deleteResult = await InitDatabaseService().Delete(id);
			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(id, deleteResult.Entry.Id);
			Assert.Equal(value, deleteResult.Entry.Value);
		}

		[Fact]
		public async void Exists()
		{
			if (Skip)
			{
				return;
			}

			var id = Guid.NewGuid().ToString();
			const string value = "my new create value";
			var createResult = await InitDatabaseService().Create(new Entry(id, value));
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.Equal(id, createResult.Entry.Id);
			Assert.Equal(value, createResult.Entry.Value);

			var existsResult = await InitDatabaseService().Exists(id);
			Assert.Equal(ExistsResult.Exists, existsResult);
		}

		[Fact]
		public async void List()
		{
			if (Skip)
			{
				return;
			}

			var id1 = Guid.NewGuid().ToString();
			const string value1 = "my new create value 1";
			var result1 = await InitDatabaseService().Create(new Entry(id1, value1));
			Assert.Equal(CreateResult.Created, result1.Result);
			Assert.Equal(id1, result1.Entry.Id);
			Assert.Equal(value1, result1.Entry.Value);

			var id2 = Guid.NewGuid().ToString();
			const string value2 = "my new create value 2";
			var result2 = await InitDatabaseService().Create(new Entry(id2, value2));
			Assert.Equal(CreateResult.Created, result2.Result);
			Assert.Equal(id2, result2.Entry.Id);
			Assert.Equal(value2, result2.Entry.Value);

			var listResult = await InitDatabaseService().List();
			Assert.Equal(ListResult.Completed, listResult.Result);
			Assert.Contains(id1, listResult.Entries);
			Assert.Contains(id2, listResult.Entries);
		}

		[Fact]
		public async void Read()
		{
			if (Skip)
			{
				return;
			}

			var id = Guid.NewGuid().ToString();
			const string value = "my new create value";
			var createResult = await InitDatabaseService().Create(new Entry(id, value));
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.Equal(id, createResult.Entry.Id);
			Assert.Equal(value, createResult.Entry.Value);

			var readResult = await InitDatabaseService().Read(id);
			Assert.Equal(ReadResult.Read, readResult.Result);
			Assert.Equal(id, readResult.Entry.Id);
			Assert.Equal(value, readResult.Entry.Value);
		}

		[Fact]
		public async void Update()
		{
			if (Skip)
			{
				return;
			}

			var id = Guid.NewGuid().ToString();
			const string value = "my new create value";
			var createResult = await InitDatabaseService().Create(new Entry(id, value));
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.Equal(id, createResult.Entry.Id);
			Assert.Equal(value, createResult.Entry.Value);

			const string newValue = value + "NEW";
			var updateResult = await InitDatabaseService().Update(new Entry(id, newValue));
			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(id, updateResult.Entry.Id);
			Assert.Equal(newValue, updateResult.Entry.Value);

			var readResult = await InitDatabaseService().Read(id);
			Assert.Equal(ReadResult.Read, readResult.Result);
			Assert.Equal(id, readResult.Entry.Id);
			Assert.Equal(newValue, readResult.Entry.Value);
		}

		private static IDatabaseService<Entry, string> InitDatabaseService()
		{
			if (databaseService != null)
			{
				return databaseService;
			}

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.test.json", false)
				.Build();
			var mongoDbConfiguration = config.GetSection("MongoDb").Get<MongoDbConfiguration>();
			databaseService = new MongoDbService<Entry, string>(
				new LoggerMock(),
				new EntryValidatorMock(),
				mongoDbConfiguration);

			return databaseService;
		}
	}
}