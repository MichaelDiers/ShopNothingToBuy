namespace User.Tests
{
	using System;
	using Application.Contracts;
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
		[InlineData(
			null,
			"applicationId",
			Roles.Admin,
			CreateResult.InvalidData)]
		[InlineData(
			"",
			"applicationId",
			Roles.Admin,
			CreateResult.InvalidData)]
		[InlineData(
			"a",
			"applicationId",
			Roles.Admin,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			Roles.Admin,
			CreateResult.Created)]
		[InlineData(
			"userId",
			"applicationId",
			Roles.None,
			CreateResult.InvalidData)]
		public async void Create(
			string userId,
			string applicationId,
			Roles roles,
			CreateResult expectedResult)
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = applicationId,
				Id = userId,
				Roles = roles
			};

			var service = InitUserService();
			var result = await service.Create(createUser);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == CreateResult.Created)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(userId.ToUpper(), result.Entry.Id);
				Assert.Equal(applicationId, result.Entry.ApplicationId);
				Assert.Equal(roles, result.Entry.Roles);
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
				Id = userId,
				Roles = Roles.Admin
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
				Assert.Equal(Roles.Admin, deleteResult.Entry.Roles);
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
				Id = userId,
				Roles = Roles.Admin
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
				Id = Guid.NewGuid().ToString()[..20],
				Roles = Roles.Admin
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
				Id = userId,
				Roles = Roles.Reader
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
				Assert.Equal(Roles.Reader, result.Entry.Roles);
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
			Roles.Reader,
			Roles.Writer,
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"",
			"applicationId",
			"userId",
			Roles.Reader,
			Roles.Writer,
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"a",
			"applicationId",
			"userId",
			Roles.Reader,
			Roles.Writer,
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"USERID",
			"applicationId",
			"userId",
			Roles.Reader,
			Roles.Writer,
			UpdateResult.Updated)]
		[InlineData(
			"userId",
			"applicationId",
			"userid",
			"applicationId",
			"userId",
			Roles.Reader,
			Roles.Writer,
			UpdateResult.Updated)]
		[InlineData(
			"userId",
			"applicationId",
			"userid",
			null,
			"userId",
			Roles.Reader,
			Roles.Writer,
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"userid",
			"",
			"userId",
			Roles.Reader,
			Roles.Writer,
			UpdateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			"userid",
			"applicationId",
			"userId",
			Roles.Reader,
			Roles.None,
			UpdateResult.InvalidData)]
		public async void Update(
			string userId,
			string applicationId,
			string updateUserId,
			string updateApplicationId,
			string updateOriginalId,
			Roles rolesCreate,
			Roles rolesUpdate,
			UpdateResult expectedResult)
		{
			var createUser = new CreateUserEntry
			{
				ApplicationId = applicationId,
				Id = userId,
				Roles = rolesCreate
			};

			var service = InitUserService();
			var _ = await service.Create(createUser);

			var updateEntry = new UpdateUserEntry
			{
				ApplicationId = updateApplicationId,
				Id = updateUserId,
				OriginalId = updateOriginalId,
				Roles = rolesUpdate
			};

			var result = await service.Update(updateEntry);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == UpdateResult.Updated)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(userId.ToUpper(), result.Entry.Id);
				Assert.Equal(updateApplicationId, result.Entry.ApplicationId);
				Assert.Equal(updateOriginalId, result.Entry.OriginalId);
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
			var service = InitUserService();

			var createUser = new CreateUserEntry
			{
				ApplicationId = Guid.NewGuid().ToString(),
				Id = "Name",
				Roles = Roles.Admin
			};
			var createResult = await service.Create(createUser);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var updateUser = new UpdateUserEntry
			{
				Id = createResult.Entry.Id,
				ApplicationId = createResult.Entry.ApplicationId + "Z",
				Roles = Roles.Reader
			};
			var updateResult = await service.Update(updateUser);
			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(createResult.Entry.Id, updateResult.Entry.Id);
			Assert.Equal(Roles.Reader, updateResult.Entry.Roles);

			var deleteResult = await service.Delete(createResult.Entry.Id);
			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(createResult.Entry.Id, deleteResult.Entry.Id);
			Assert.Equal(Roles.Reader, deleteResult.Entry.Roles);
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