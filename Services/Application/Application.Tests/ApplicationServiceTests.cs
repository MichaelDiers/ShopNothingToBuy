namespace Application.Tests
{
	using System;
	using Application.Contracts;
	using Application.Services;
	using Application.Services.Models;
	using Application.Tests.Models;
	using Service.Sdk.Contracts;
	using Xunit;

	public class ApplicationServiceTests
	{
		[Fact]
		public async void Clear_ShouldSucceed()
		{
			var service = InitService();
			var result = await service.Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Fact]
		public async void Create_ShouldSucceed()
		{
			var entry = new CreateApplicationEntry
			{
				Name = Guid.NewGuid().ToString()
			};

			var service = InitService();
			var result = await service.Create(entry);
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
			var service = InitService();
			var result = await service.Delete(id);
			Assert.Equal(DeleteResult.Deleted, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void Exists_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var service = InitService();
			var result = await service.Exists(id);
			Assert.Equal(ExistsResult.Exists, result);
		}

		[Fact]
		public async void List_ShouldSucceed()
		{
			var service = InitService();
			var result = await service.List();
			Assert.Equal(ListResult.Completed, result.Result);
			Assert.NotNull(result.Entries);
			Assert.Empty(result.Entries);
		}

		[Fact]
		public async void Read_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var service = InitService();
			var result = await service.Read(id);
			Assert.Equal(ReadResult.Read, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void Update_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var updateEntry = new UpdateApplicationEntry
			{
				Id = id,
				Name = "name"
			};

			var service = InitService();
			var result = await service.Update(updateEntry);
			Assert.Equal(UpdateResult.Updated, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void WorkflowTest()
		{
			var service = InitService();

			var entry = new CreateApplicationEntry
			{
				Name = "Name"
			};
			var createResult = await service.Create(entry);
			Assert.Equal(CreateResult.Created, createResult.Result);

			var updateEntry = new UpdateApplicationEntry
			{
				Id = createResult.Entry.Id,
				Name = createResult.Entry.Name + "2"
			};
			var updateResult = await service.Update(updateEntry);
			Assert.Equal(UpdateResult.Updated, updateResult.Result);
			Assert.Equal(createResult.Entry.Id, updateResult.Entry.Id);

			var deleteResult = await service.Delete(createResult.Entry.Id);
			Assert.Equal(DeleteResult.Deleted, deleteResult.Result);
			Assert.Equal(createResult.Entry.Id, deleteResult.Entry.Id);
		}

		private static IApplicationService InitService()
		{
			return new ApplicationService(new LoggerMock(), new ApplicationServiceValidator(), new DatabaseServiceMock());
		}
	}
}