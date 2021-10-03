namespace Services.Crud.User.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using global::Services.Crud.User.Services;
	using global::Services.Crud.User.Tests.Mocks;
	using Service.Sdk.Contracts.Crud.Application;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Contracts.Crud.User;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Mocks;
	using Xunit;

	public class UserServiceTests
	{
		[Fact]
		public async void Clear()
		{
			var service = InitService();
			var result = await service.Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Theory]
		[InlineData(
			null,
			"applicationId",
			Roles.Admin,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"",
			"applicationId",
			Roles.Admin,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"a",
			"applicationId",
			Roles.Admin,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"aa",
			"applicationId",
			Roles.Admin,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
			"applicationId",
			Roles.Admin,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"invalidApplicationId",
			Roles.Admin,
			ReadResult.InvalidData,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId",
			Roles.Reader,
			ReadResult.Read,
			CreateResult.Created)]
		public async void Create(
			string userId,
			string applicationId,
			Roles applicationRoles,
			ReadResult applicationReadResult,
			CreateResult expectedResult)
		{
			var entry = new CreateUserEntry
			{
				Id = userId,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId, applicationRoles)
				}
			};

			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						applicationReadResult,
						new ApplicationEntry(applicationId.ToUpper(), applicationId, applicationRoles))
				});

			var result = await service.Create(entry);
			Assert.Equal(expectedResult, result.Result);
			if (expectedResult == CreateResult.Created)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(userId.ToUpper(), result.Entry.Id);
				Assert.Equal(userId, result.Entry.OriginalId);
				Assert.Single(result.Entry.Applications);
				Assert.Equal(result.Entry.Applications.First().Roles, applicationRoles);
				Assert.Equal(result.Entry.Applications.First().ApplicationId, applicationId);
			}
		}

		[Fact]
		public async void CreateExistingUserShouldFail()
		{
			var entry = new CreateUserEntry
			{
				Id = "userId",
				Applications = new[]
				{
					new UserApplicationEntry("applicationId", Roles.Admin)
				}
			};

			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry("applicationId", "applicationId", Roles.Admin))
				});

			var result = await service.Create(entry);
			Assert.Equal(CreateResult.Created, result.Result);

			result = await service.Create(entry);
			Assert.Equal(CreateResult.AlreadyExists, result.Result);
		}

		[Theory]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.All,
			Roles.Reader,
			"applicationId2",
			Roles.All,
			Roles.Writer,
			ReadResult.Read,
			ReadResult.Read,
			CreateResult.Created)]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.All,
			Roles.Reader,
			"applicationId2",
			Roles.All,
			Roles.Writer,
			ReadResult.InvalidData,
			ReadResult.InvalidData,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.All,
			Roles.Reader,
			"applicationId2",
			Roles.All,
			Roles.Writer,
			ReadResult.Read,
			ReadResult.InvalidData,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.All,
			Roles.Reader,
			"applicationId2",
			Roles.All,
			Roles.Writer,
			ReadResult.InvalidData,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.All,
			Roles.Reader,
			"applicationId2",
			Roles.Reader,
			Roles.Writer,
			ReadResult.Read,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.All,
			Roles.Reader,
			"applicationId2",
			Roles.Reader,
			Roles.Reader,
			ReadResult.Read,
			ReadResult.Read,
			CreateResult.Created)]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.Reader,
			Roles.All,
			"applicationId2",
			Roles.Reader,
			Roles.Reader,
			ReadResult.Read,
			ReadResult.Read,
			CreateResult.InvalidData)]
		[InlineData(
			"userId",
			"applicationId1",
			Roles.Reader,
			Roles.Reader,
			"applicationId2",
			Roles.Reader,
			Roles.Reader,
			ReadResult.Read,
			ReadResult.Read,
			CreateResult.Created)]
		public async void CreateWithMultipleApplications(
			string userId,
			string applicationId1,
			Roles applicationRoles1,
			Roles applicationRolesUser1,
			string applicationId2,
			Roles applicationRoles2,
			Roles applicationRolesUser2,
			ReadResult applicationReadResult1,
			ReadResult applicationReadResult2,
			CreateResult expectedReadResult)
		{
			var entry = new CreateUserEntry
			{
				Id = userId,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId1, applicationRolesUser1),
					new UserApplicationEntry(applicationId2, applicationRolesUser2)
				}
			};

			var service = InitService(
				new[]
				{
					applicationReadResult1 == ReadResult.Read
						? new OperationResult<ApplicationEntry, string, ReadResult>(
							applicationReadResult1,
							new ApplicationEntry(applicationId1, applicationId1, applicationRoles1))
						: new OperationResult<ApplicationEntry, string, ReadResult>(applicationReadResult1),
					applicationReadResult2 == ReadResult.Read
						? new OperationResult<ApplicationEntry, string, ReadResult>(
							applicationReadResult2,
							new ApplicationEntry(applicationId2, applicationId2, applicationRoles2))
						: new OperationResult<ApplicationEntry, string, ReadResult>(applicationReadResult2)
				});
			var result = await service.Create(entry);
			Assert.Equal(expectedReadResult, result.Result);
			if (expectedReadResult == CreateResult.Created)
			{
				Assert.NotNull(result.Entry);
				Assert.Equal(userId.ToUpper(), result.Entry.Id);
				Assert.Equal(userId, result.Entry.OriginalId);
				Assert.Equal(2, result.Entry.Applications.Count());
				Assert.Contains(
					result.Entry.Applications,
					application => application.ApplicationId == applicationId1 && application.Roles == applicationRolesUser1);
				Assert.Contains(
					result.Entry.Applications,
					application => application.ApplicationId == applicationId2 && application.Roles == applicationRolesUser2);
			}
		}

		[Fact]
		public async void ExistingEntry()
		{
			const string applicationId = "applicationId";
			const Roles applicationRoles = Roles.ReaderWriter;
			const string userId = "userId";

			var createEntry = new CreateUserEntry
			{
				Id = userId,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId, applicationRoles)
				}
			};

			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry(applicationId.ToUpper(), applicationId, applicationRoles))
				});
			var createResult = await service.Create(createEntry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var existsResult = await service.Exists(userId);
			Assert.Equal(ExistsResult.Exists, existsResult);

			var readResult = await service.Read(userId);
			Assert.Equal(ReadResult.Read, readResult.Result);
			Assert.Equal(userId, readResult.Entry.OriginalId);
			Assert.Equal(userId.ToUpper(), readResult.Entry.Id);
			Assert.Single(readResult.Entry.Applications);
			Assert.Equal(applicationId, readResult.Entry.Applications.First().ApplicationId);
			Assert.True((readResult.Entry.Applications.First().Roles & applicationRoles) == applicationRoles);

			var readListResult = (await service.Read(
				new[]
				{
					userId
				})).ToArray();
			Assert.Equal(ReadResult.Read, readListResult.First().Result);
			Assert.Equal(userId, readListResult.First().Entry.OriginalId);
			Assert.Equal(userId.ToUpper(), readListResult.First().Entry.Id);
			Assert.Single(readListResult.First().Entry.Applications);
			Assert.Equal(applicationId, readListResult.First().Entry.Applications.First().ApplicationId);
			Assert.True((readListResult.First().Entry.Applications.First().Roles & applicationRoles) == applicationRoles);

			var deleteResult = await service.Delete(userId);
			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(userId, deleteResult.Entry.OriginalId);
			Assert.Equal(userId.ToUpper(), deleteResult.Entry.Id);
			Assert.Single(deleteResult.Entry.Applications);
			Assert.Equal(applicationId, deleteResult.Entry.Applications.First().ApplicationId);
			Assert.True((deleteResult.Entry.Applications.First().Roles & applicationRoles) == applicationRoles);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("a")]
		[InlineData("aa")]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
		public async void InvalidEntryId(string userId)
		{
			var service = InitService();
			var deleteResult = await service.Delete(userId);
			Assert.Equal(DeleteResult.InvalidData, deleteResult.Result);

			var existsResult = await service.Exists(userId);
			Assert.Equal(ExistsResult.InvalidData, existsResult);

			var readResult = await service.Read(userId);
			Assert.Equal(ReadResult.InvalidData, readResult.Result);

			var readListResult = await service.Read(
				new[]
				{
					userId
				});
			Assert.Equal(ReadResult.InvalidData, readListResult.First().Result);
		}

		[Fact]
		public async void List()
		{
			const string applicationId = "applicationId";
			const Roles applicationRoles = Roles.ReaderWriter;
			const string userId = "userId";

			var createEntry = new CreateUserEntry
			{
				Id = userId,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId, applicationRoles)
				}
			};

			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry(applicationId.ToUpper(), applicationId, applicationRoles))
				});
			var createResult = await service.Create(createEntry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var listResult = await service.List();
			Assert.Equal(ListResult.Completed, listResult.Result);
			Assert.Contains(listResult.Entries, id => id == userId.ToUpper());
		}

		[Fact]
		public async void MissingEntry()
		{
			const string userId = "userId";

			var service = InitService();
			var deleteResult = await service.Delete(userId);
			Assert.Equal(DeleteResult.NotFound, deleteResult.Result);

			var existsResult = await service.Exists(userId);
			Assert.Equal(ExistsResult.NotFound, existsResult);

			var readResult = await service.Read(userId);
			Assert.Equal(ReadResult.NotFound, readResult.Result);

			var readListResult = await service.Read(
				new[]
				{
					userId
				});
			Assert.Equal(ReadResult.NotFound, readListResult.First().Result);
		}

		[Fact]
		public async void ReadEntries()
		{
			const string applicationId = "applicationId";
			const Roles applicationRoles = Roles.ReaderWriter;
			const string userId = "userId";

			var createEntry = new CreateUserEntry
			{
				Id = userId,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId, applicationRoles)
				}
			};

			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry(applicationId.ToUpper(), applicationId, applicationRoles))
				});
			var createResult = await service.Create(createEntry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var readResult = (await service.Read(
				new[]
				{
					userId
				})).ToArray();
			Assert.Equal(ReadResult.Read, readResult.First().Result);
			Assert.Equal(userId.ToUpper(), readResult.First().Entry.Id);
			Assert.Equal(userId, readResult.First().Entry.OriginalId);
			Assert.Equal(applicationRoles, readResult.First().Entry.Applications.First().Roles);
			Assert.Equal(applicationId, readResult.First().Entry.Applications.First().ApplicationId);
		}

		[Fact]
		public async void ReadEntry()
		{
			const string applicationId = "applicationId";
			const Roles applicationRoles = Roles.ReaderWriter;
			const string userId = "userId";

			var createEntry = new CreateUserEntry
			{
				Id = userId,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId, applicationRoles)
				}
			};

			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry(applicationId.ToUpper(), applicationId, applicationRoles))
				});
			var createResult = await service.Create(createEntry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var readResult = await service.Read(userId);
			Assert.Equal(ReadResult.Read, readResult.Result);
			Assert.Equal(userId.ToUpper(), readResult.Entry.Id);
			Assert.Equal(userId, readResult.Entry.OriginalId);
			Assert.Equal(applicationRoles, readResult.Entry.Applications.First().Roles);
			Assert.Equal(applicationId, readResult.Entry.Applications.First().ApplicationId);
		}

		[Theory]
		[InlineData(null, "applicationId", Roles.Admin)]
		[InlineData("", "applicationId", Roles.Admin)]
		[InlineData("a", "applicationId", Roles.Admin)]
		[InlineData("aa", "applicationId", Roles.Admin)]
		[InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "applicationId", Roles.Admin)]
		public async void Update(string userId, string applicationId, Roles applicationRoles)
		{
			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry(applicationId.ToUpper(), applicationId, applicationRoles))
				});
			var result = await service.Update(
				new UpdateUserEntry
				{
					Id = userId,
					Applications = new[]
					{
						new UserApplicationEntry(applicationId, applicationRoles)
					}
				});
			Assert.Equal(UpdateResult.InvalidData, result.Result);
		}

		[Fact]
		public async void UpdateEntryExists()
		{
			const string applicationId = "applicationId";
			const Roles applicationRoles = Roles.ReaderWriter;
			const string userId = "userId";

			var createEntry = new CreateUserEntry
			{
				Id = userId,
				Applications = new[]
				{
					new UserApplicationEntry(applicationId, applicationRoles)
				}
			};

			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry(applicationId.ToUpper(), applicationId, applicationRoles))
				});
			var createResult = await service.Create(createEntry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var newRoles = Roles.Reader;

			var updateResult = await service.Update(
				new UpdateUserEntry
				{
					Id = userId,
					OriginalId = userId,
					Applications = new[]
					{
						new UserApplicationEntry(applicationId, newRoles)
					}
				});
			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(userId.ToUpper(), updateResult.Entry.Id);
			Assert.Equal(userId, updateResult.Entry.OriginalId);
			Assert.Equal(newRoles, updateResult.Entry.Applications.First().Roles);
			Assert.Equal(applicationId, updateResult.Entry.Applications.First().ApplicationId);
		}

		[Fact]
		public async void UpdateMissingEntry()
		{
			var service = InitService(
				new[]
				{
					new OperationResult<ApplicationEntry, string, ReadResult>(
						ReadResult.Read,
						new ApplicationEntry("applicationId".ToUpper(), "applicationId", Roles.Admin))
				});
			var result = await service.Update(
				new UpdateUserEntry
				{
					Id = "userId",
					Applications = new[]
					{
						new UserApplicationEntry("applicationId", Roles.Admin)
					}
				});
			Assert.Equal(UpdateResult.NotFound, result.Result);
		}

		private static IUserService InitService()
		{
			return InitService(null);
		}

		private static IUserService InitService(
			IEnumerable<IOperationResult<ApplicationEntry, string, ReadResult>> applicationReadResult)
		{
			return new UserService(
				new LoggerMock(),
				new UserServiceValidator(),
				new InMemoryCrudServiceStringIdMock<UserEntry>(),
				new ApplicationServiceMock(applicationReadResult));
		}
	}
}