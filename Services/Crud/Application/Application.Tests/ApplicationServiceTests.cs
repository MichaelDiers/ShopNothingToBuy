namespace Application.Tests
{
	using Application.Services;
	using Application.Tests.Models;
	using Service.Contracts.Crud.Application;
	using Service.Contracts.Crud.Base;
	using Service.Models.Crud.Application;
	using Xunit;

	public class ApplicationServiceTests
	{
		[Fact]
		public async void Clear()
		{
			var service = InitService();
			var result = await service.Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Theory]
		[InlineData(null, Roles.Admin, CreateResult.InvalidData)]
		[InlineData("", Roles.Admin, CreateResult.InvalidData)]
		[InlineData("a", Roles.Admin, CreateResult.InvalidData)]
		[InlineData("aa", Roles.Admin, CreateResult.InvalidData)]
		[InlineData("aaa", Roles.Admin, CreateResult.Created)]
		[InlineData("AAA", Roles.Admin, CreateResult.Created)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Roles.Admin, CreateResult.Created)]
		[InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", Roles.Admin, CreateResult.Created)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Roles.Admin, CreateResult.InvalidData)]
		[InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", Roles.Admin, CreateResult.InvalidData)]
		[InlineData("aaa", Roles.None, CreateResult.InvalidData)]
		public async void Create(string id, Roles roles, CreateResult expectedResult)
		{
			var entry = new CreateApplicationEntry
			{
				Id = id,
				Roles = roles
			};

			var service = InitService();
			var result = await service.Create(entry);
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
		[InlineData(
			"applicationId",
			Roles.Admin,
			null,
			DeleteResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"",
			DeleteResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aa",
			DeleteResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aas",
			DeleteResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			DeleteResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			DeleteResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			DeleteResult.Deleted)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"APPLICATIONID",
			DeleteResult.Deleted)]
		public async void Delete(
			string id,
			Roles roles,
			string requestId,
			DeleteResult expectedResult)
		{
			var entry = new CreateApplicationEntry
			{
				Id = id,
				Roles = roles
			};

			var service = InitService();
			await service.Create(entry);

			var result = await service.Delete(requestId);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == DeleteResult.Deleted)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
				Assert.Equal(roles, result.Entry.Roles);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Theory]
		[InlineData(
			"applicationId",
			Roles.Admin,
			null,
			ExistsResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"",
			ExistsResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aa",
			ExistsResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aas",
			ExistsResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			ExistsResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			ExistsResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			ExistsResult.Exists)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"APPLICATIONID",
			ExistsResult.Exists)]
		public async void Exists(
			string id,
			Roles roles,
			string requestId,
			ExistsResult expectedResult)
		{
			var entry = new CreateApplicationEntry
			{
				Id = id,
				Roles = roles
			};

			var service = InitService();
			await service.Create(entry);

			var result = await service.Exists(requestId);
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public async void List()
		{
			const string id = "applicationId";
			var entry = new CreateApplicationEntry
			{
				Id = id,
				Roles = Roles.Admin
			};

			var service = InitService();
			await service.Create(entry);

			var result = await service.List();
			Assert.Equal(ListResult.Completed, result.Result);
			Assert.NotNull(result.Entries);
			Assert.Contains(id.ToUpper(), result.Entries);
		}

		[Theory]
		[InlineData(
			"applicationId",
			Roles.Admin,
			null,
			ReadResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"",
			ReadResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aa",
			ReadResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aas",
			ReadResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			ReadResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			ReadResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			ReadResult.Read)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"APPLICATIONID",
			ReadResult.Read)]
		public async void Read(
			string id,
			Roles roles,
			string requestId,
			ReadResult expectedResult)
		{
			var entry = new CreateApplicationEntry
			{
				Id = id,
				Roles = roles
			};

			var service = InitService();
			await service.Create(entry);

			var result = await service.Read(requestId);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == ReadResult.Read)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(id.ToUpper(), result.Entry.Id);
				Assert.Equal(roles, result.Entry.Roles);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Theory]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			"applicationId",
			Roles.Admin,
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			"applicationID",
			Roles.Admin,
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			"APPLICATIONID",
			Roles.Admin,
			UpdateResult.Updated)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			null,
			null,
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"",
			"",
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aa",
			"aa",
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaa",
			"aaa",
			Roles.Admin,
			UpdateResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			Roles.Admin,
			UpdateResult.NotFound)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			"application",
			Roles.Admin,
			UpdateResult.InvalidData)]
		[InlineData(
			"applicationId",
			Roles.Admin,
			"applicationId",
			"APPLICATIONID",
			Roles.None,
			UpdateResult.InvalidData)]
		public async void Update(
			string idCreate,
			Roles rolesCreate,
			string idUpdate,
			string originalIdUpdate,
			Roles rolesUpdate,
			UpdateResult expectedResult)
		{
			var entry = new CreateApplicationEntry
			{
				Id = idCreate,
				Roles = rolesCreate
			};

			var service = InitService();
			await service.Create(entry);

			var updateEntry = new UpdateApplicationEntry
			{
				Id = idUpdate,
				OriginalId = originalIdUpdate,
				Roles = rolesUpdate
			};

			var result = await service.Update(updateEntry);
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

		[Fact]
		public async void WorkflowTest()
		{
			var service = InitService();
			const string id = "applicationid";
			const Roles roles = Roles.Admin;

			var entry = new CreateApplicationEntry
			{
				Id = id,
				Roles = roles
			};
			var createResult = await service.Create(entry);
			Assert.Equal(CreateResult.Created, createResult.Result);
			Assert.Equal(id, createResult.Entry.OriginalId);
			Assert.Equal(id.ToUpper(), createResult.Entry.Id);
			Assert.Equal(roles, createResult.Entry.Roles);

			var updateEntry = new UpdateApplicationEntry
			{
				Id = id,
				OriginalId = id.ToUpper(),
				Roles = roles
			};
			var updateResult = await service.Update(updateEntry);
			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(id.ToUpper(), updateResult.Entry.Id);
			Assert.Equal(id.ToUpper(), updateResult.Entry.OriginalId);
			Assert.Equal(roles, updateResult.Entry.Roles);

			var existsResult = await service.Exists(id);
			Assert.Equal(ExistsResult.Exists, existsResult);

			var listResult = await service.List();
			Assert.Equal(ListResult.Completed, listResult.Result);
			Assert.Contains(id.ToUpper(), listResult.Entries);

			var deleteResult = await service.Delete(id);
			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(id.ToUpper(), deleteResult.Entry.Id);
			Assert.Equal(id.ToUpper(), deleteResult.Entry.OriginalId);
			Assert.Equal(roles, deleteResult.Entry.Roles);

			var clearResult = await service.Clear();
			Assert.Equal(ClearResult.Cleared, clearResult);
		}

		private static IApplicationService InitService()
		{
			return new ApplicationService(new LoggerMock(), new ApplicationServiceValidator(), new DatabaseServiceMock());
		}
	}
}