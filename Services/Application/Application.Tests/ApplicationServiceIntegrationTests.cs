namespace Application.Tests
{
	using Application.Contracts;
	using Application.Services;
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
		public async void Clear()
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

		[Theory]
		[InlineData(null, CreateResult.InvalidData)]
		[InlineData("", CreateResult.InvalidData)]
		[InlineData("a", CreateResult.InvalidData)]
		[InlineData("aa", CreateResult.InvalidData)]
		[InlineData("aaa", CreateResult.Created)]
		[InlineData("AAA", CreateResult.Created)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", CreateResult.Created)]
		[InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", CreateResult.Created)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", CreateResult.InvalidData)]
		[InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", CreateResult.InvalidData)]
		public async void Create(string id, CreateResult expectedResult)
		{
			if (Skip)
			{
				return;
			}

			var applicationService = InitService();
			await applicationService.Clear();

			var entry = new CreateApplicationEntry
			{
				Id = id
			};

			var result = await applicationService.Create(entry);

			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == CreateResult.Created)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
				Assert.Equal(id, result.Entry.OriginalId);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Theory]
		[InlineData("applicationId", null, DeleteResult.InvalidData)]
		[InlineData("applicationId", "", DeleteResult.InvalidData)]
		[InlineData("applicationId", "aa", DeleteResult.InvalidData)]
		[InlineData("applicationId", "aas", DeleteResult.NotFound)]
		[InlineData("applicationId", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", DeleteResult.NotFound)]
		[InlineData("applicationId", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", DeleteResult.InvalidData)]
		[InlineData("applicationId", "applicationId", DeleteResult.Deleted)]
		[InlineData("applicationId", "APPLICATIONID", DeleteResult.Deleted)]
		public async void Delete(string id, string requestId, DeleteResult expectedResult)
		{
			if (Skip)
			{
				return;
			}

			var entry = new CreateApplicationEntry
			{
				Id = id
			};

			var applicationService = InitService();
			await applicationService.Create(entry);

			var result = await applicationService.Delete(requestId);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == DeleteResult.Deleted)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Theory]
		[InlineData("applicationId", null, ExistsResult.InvalidData)]
		[InlineData("applicationId", "", ExistsResult.InvalidData)]
		[InlineData("applicationId", "aa", ExistsResult.InvalidData)]
		[InlineData("applicationId", "aas", ExistsResult.NotFound)]
		[InlineData("applicationId", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", ExistsResult.NotFound)]
		[InlineData("applicationId", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", ExistsResult.InvalidData)]
		[InlineData("applicationId", "applicationId", ExistsResult.Exists)]
		[InlineData("applicationId", "APPLICATIONID", ExistsResult.Exists)]
		public async void Exists(string id, string requestId, ExistsResult expectedResult)
		{
			if (Skip)
			{
				return;
			}

			var entry = new CreateApplicationEntry
			{
				Id = id
			};

			var applicationService = InitService();
			await applicationService.Clear();

			await applicationService.Create(entry);

			var result = await applicationService.Exists(requestId);
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public async void List()
		{
			if (Skip)
			{
				return;
			}

			const string id = "applicationId";
			var entry = new CreateApplicationEntry
			{
				Id = id
			};

			var applicationService = InitService();
			await applicationService.Delete(id);

			await applicationService.Create(entry);

			var result = await applicationService.List();
			Assert.Equal(ListResult.Completed, result.Result);
			Assert.NotNull(result.Entries);
			Assert.Contains(id.ToUpper(), result.Entries);
		}

		[Theory]
		[InlineData("applicationId", null, ReadResult.InvalidData)]
		[InlineData("applicationId", "", ReadResult.InvalidData)]
		[InlineData("applicationId", "aa", ReadResult.InvalidData)]
		[InlineData("applicationId", "aas", ReadResult.NotFound)]
		[InlineData("applicationId", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", ReadResult.NotFound)]
		[InlineData("applicationId", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", ReadResult.InvalidData)]
		[InlineData("applicationId", "applicationId", ReadResult.Read)]
		[InlineData("applicationId", "APPLICATIONID", ReadResult.Read)]
		public async void Read(string id, string requestId, ReadResult expectedResult)
		{
			if (Skip)
			{
				return;
			}

			var entry = new CreateApplicationEntry
			{
				Id = id
			};

			var applicationService = InitService();
			await applicationService.Create(entry);

			var result = await applicationService.Read(requestId);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == ReadResult.Read)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Theory]
		[InlineData(
			"applicationId",
			"applicationId",
			"applicationId",
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			"applicationId",
			"applicationID",
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			"applicationId",
			"APPLICATIONID",
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			null,
			null,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"",
			"",
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"aa",
			"aa",
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"aaa",
			"aaa",
			UpdateResult.NotFound)]
		[InlineData(
			"applicationId",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			UpdateResult.NotFound)]
		[InlineData(
			"applicationId",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"applicationId",
			"application",
			UpdateResult.InvalidData)]
		public async void Update(
			string idCreate,
			string idUpdate,
			string originalIdUpdate,
			UpdateResult expectedResult)
		{
			var entry = new CreateApplicationEntry
			{
				Id = idCreate
			};

			var applicationService = InitService();
			await applicationService.Create(entry);

			var updateEntry = new UpdateApplicationEntry
			{
				Id = idUpdate,
				OriginalId = originalIdUpdate
			};

			var result = await applicationService.Update(updateEntry);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == UpdateResult.Updated)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(idCreate.ToUpper(), result.Entry.Id);
				Assert.Equal(originalIdUpdate, result.Entry.OriginalId);
			}
			else
			{
				Assert.Null(result.Entry);
			}
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