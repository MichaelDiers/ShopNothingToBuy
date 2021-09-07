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
		public async void Clear()
		{
			var service = InitUserService();
			var result = await service.Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Theory]
		[InlineData(null, "applicationId", CreateResult.InvalidData)]
		[InlineData("", "applicationId", CreateResult.InvalidData)]
		[InlineData("a", "applicationId", CreateResult.InvalidData)]
		[InlineData("userId", "applicationId", CreateResult.Created)]
		public async void Create(string userId, string applicationId, CreateResult expectedResult)
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = applicationId,
				Id = userId
			};

			var service = InitUserService();
			var result = await service.Create(createUser);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == CreateResult.Created)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(userId.ToUpper(), result.Entry.Id);
				Assert.Equal(applicationId, result.Entry.ApplicationId);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Theory]
		[InlineData(
			"DeleteUser",
			"DELETEUSER",
			"ApplicationId",
			DeleteResult.Deleted)]
		[InlineData(
			"DeleteUser",
			"DeleteUser",
			"ApplicationId",
			DeleteResult.Deleted)]
		[InlineData(
			"DeleteUser",
			null,
			"ApplicationId",
			DeleteResult.InvalidData)]
		[InlineData(
			"DeleteUser",
			"",
			"ApplicationId",
			DeleteResult.InvalidData)]
		[InlineData(
			"DeleteUser",
			"zzzzzzzzzz",
			"ApplicationId",
			DeleteResult.NotFound)]
		public async void Delete(
			string userId,
			string requestId,
			string applicationId,
			DeleteResult expectedDeleteResult)
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = applicationId,
				Id = userId
			};

			var service = InitUserService();
			await service.Clear();
			await service.Create(createUser);

			var deleteResult = await service.Delete(requestId);
			Assert.Equal(expectedDeleteResult, deleteResult.Result);

			if (expectedDeleteResult == DeleteResult.Deleted)
			{
				Assert.NotNull(deleteResult.Entry);
				Assert.Equal(userId?.ToUpper(), deleteResult.Entry.Id);
				Assert.Equal(userId, deleteResult.Entry.OriginalId);
				Assert.Equal(applicationId, deleteResult.Entry.ApplicationId);
			}
			else
			{
				Assert.Null(deleteResult.Entry);
			}
		}

		[Theory]
		[InlineData(
			"userId",
			null,
			"applicationId",
			ExistsResult.InvalidData)]
		[InlineData(
			"userId",
			"",
			"applicationId",
			ExistsResult.InvalidData)]
		[InlineData(
			"userId",
			"a",
			"applicationId",
			ExistsResult.InvalidData)]
		[InlineData(
			"userId",
			"USERID",
			"applicationId",
			ExistsResult.Exists)]
		public async void Exists(
			string userId,
			string requestId,
			string applicationId,
			ExistsResult expectedResult)
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = applicationId,
				Id = userId
			};

			var service = InitUserService();
			var _ = await service.Create(createUser);

			var result = await service.Exists(requestId);
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public async void List()
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = Guid.NewGuid().ToString(),
				Id = Guid.NewGuid().ToString()[..20]
			};

			var service = InitUserService();
			var _ = await service.Create(createUser);

			var result = await service.List();
			Assert.Equal(ListResult.Completed, result.Result);
			Assert.NotNull(result.Entries);
			Assert.Contains(createUser.Id.ToUpper(), result.Entries);
		}

		[Theory]
		[InlineData(
			"userid",
			null,
			"applicationId",
			ReadResult.InvalidData)]
		[InlineData(
			"userid",
			"",
			"applicationId",
			ReadResult.InvalidData)]
		[InlineData(
			"userid",
			"u",
			"applicationId",
			ReadResult.InvalidData)]
		[InlineData(
			"userid",
			"userid",
			"applicationId",
			ReadResult.Read)]
		[InlineData(
			"userid",
			"USERID",
			"applicationId",
			ReadResult.Read)]
		public async void Read(
			string userId,
			string requestId,
			string applicationId,
			ReadResult expectedResult)
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = applicationId,
				Id = userId
			};

			var service = InitUserService();
			var _ = await service.Create(createUser);

			var result = await service.Read(requestId);

			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == ReadResult.Read)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(userId.ToUpper(), result.Entry.Id);
				Assert.Equal(applicationId, result.Entry.ApplicationId);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Theory]
		[InlineData(
			"userId",
			"applicationId",
			null,
			"applicationId",
			"userId",
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"",
			"applicationId",
			"userId",
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"a",
			"applicationId",
			"userId",
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"USERID",
			"applicationId",
			"userId",
			UpdateResult.Updated)]
		[InlineData(
			"userId",
			"applicationId",
			"userid",
			"applicationId",
			"userId",
			UpdateResult.Updated)]
		[InlineData(
			"userId",
			"applicationId",
			"userid",
			null,
			"userId",
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"userid",
			"",
			"userId",
			UpdateResult.InvalidData)]
		public async void Update(
			string userId,
			string applicationId,
			string updateUserId,
			string updateApplicationId,
			string updateOriginalId,
			UpdateResult expectedResult)
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = applicationId,
				Id = userId
			};

			var service = InitUserService();
			var _ = await service.Create(createUser);

			var updateEntry = new UpdateUserEntry
			{
				ApplicationId = updateApplicationId,
				Id = updateUserId,
				OriginalId = updateOriginalId
			};

			var result = await service.Update(updateEntry);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == UpdateResult.Updated)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(userId.ToUpper(), result.Entry.Id);
				Assert.Equal(updateApplicationId, result.Entry.ApplicationId);
				Assert.Equal(updateOriginalId, result.Entry.OriginalId);
			}
			else
			{
				Assert.Null(result.Entry);
			}
		}

		[Fact]
		public async void WorkflowTest()
		{
			var service = InitUserService();

			var createUser = new CreateUserEntry
			{
				ApplicationId = Guid.NewGuid().ToString(),
				Id = "Name"
			};
			var createResult = await service.Create(createUser);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var updateUser = new UpdateUserEntry
			{
				Id = createResult.Entry.Id,
				ApplicationId = createResult.Entry.ApplicationId + "Z"
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
			return new UserService(
				new LoggerMock(),
				new UserServiceValidator(),
				new DatabaseServiceMock(),
				new ApplicationServiceMock());
		}
	}
}