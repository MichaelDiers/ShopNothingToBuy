namespace Application.Tests
{
	using Application.Contracts;
	using Application.Services;
	using Application.Tests.Models;
	using Microsoft.Extensions.Configuration;
	using Service.Contracts.Crud.Application;
	using Service.Contracts.Crud.Base;
	using Service.Models.Crud.Database;
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
		[InlineData("aaa", Roles.Admin, CreateResult.Created)]
		[InlineData("AAA", Roles.Admin, CreateResult.Created)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Roles.Admin, CreateResult.Created)]
		[InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", Roles.Admin, CreateResult.Created)]
		public async void Create(string id, Roles roles, CreateResult expectedResult)
		{
			if (Skip)
			{
				return;
			}

			var applicationService = InitService();
			await applicationService.Clear();

			var entry = new CreateApplicationEntry
			{
				Id = id,
				Roles = roles
			};

			var result = await applicationService.Create(entry);

			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == CreateResult.Created)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
				Assert.Equal(id, result.Entry.OriginalId);
				Assert.Equal(roles, result.Entry.Roles);
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
				Id = id,
				Roles = Roles.Admin
			};

			var applicationService = InitService();
			await applicationService.Clear();
			await applicationService.Create(entry);

			var result = await applicationService.Delete(requestId);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == DeleteResult.Deleted)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
				Assert.Equal(Roles.Admin, result.Entry.Roles);
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
				Id = id,
				Roles = Roles.Admin
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
				Id = id,
				Roles = Roles.Admin
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
				Id = id,
				Roles = Roles.All
			};

			var applicationService = InitService();

			var clearResult = await applicationService.Clear();
			Assert.Equal(ClearResult.Cleared, clearResult);

			var createResult = await applicationService.Create(entry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var result = await applicationService.Read(requestId);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == ReadResult.Read)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
				Assert.Equal(Roles.All, result.Entry.Roles);
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
			Roles.Admin,
			Roles.All,
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			"applicationId",
			"applicationID",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			"applicationId",
			"APPLICATIONID",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			"applicationId",
			"APPLICATIONID",
			Roles.Admin,
			Roles.None,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			null,
			null,
			Roles.Admin,
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"",
			"",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"aa",
			"aa",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"aaa",
			"aaa",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.NotFound)]
		[InlineData(
			"applicationId",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.NotFound)]
		[InlineData(
			"applicationId",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			"applicationId",
			"application",
			Roles.Admin,
			Roles.Admin,
			UpdateResult.InvalidData)]
		public async void Update(
			string idCreate,
			string idUpdate,
			string originalIdUpdate,
			Roles rolesCreate,
			Roles rolesUpdate,
			UpdateResult expectedResult)
		{
			if (Skip)
			{
				return;
			}

			var entry = new CreateApplicationEntry
			{
				Id = idCreate,
				Roles = rolesCreate
			};

			var applicationService = InitService();

			var clearResult = await applicationService.Clear();
			Assert.Equal(ClearResult.Cleared, clearResult);

			var createResult = await applicationService.Create(entry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var updateEntry = new UpdateApplicationEntry
			{
				Id = idUpdate,
				OriginalId = originalIdUpdate,
				Roles = rolesUpdate
			};

			var result = await applicationService.Update(updateEntry);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == UpdateResult.Updated)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(idCreate.ToUpper(), result.Entry.Id);
				Assert.Equal(originalIdUpdate, result.Entry.OriginalId);
				Assert.Equal(rolesUpdate, result.Entry.Roles);
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