namespace Authentication.Tests.Services
{
	using System;
	using Authentication.Services;
	using Authentication.Services.Models;
	using Authentication.Tests.Mocks;
	using Service.Sdk.Contracts;
	using Xunit;

	public class AuthenticationServiceTests
	{
		[Fact]
		public async void Clear_ShouldSucceed()
		{
			var result = await new AuthenticationService(new LoggerMock()).Clear();
			Assert.Equal(ClearResult.Cleared, result);
		}

		[Fact]
		public async void Create_ShouldSucceed()
		{
			var createUser = new CreateUser();
			var result = await new AuthenticationService(new LoggerMock()).Create(createUser);
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
			var result = await new AuthenticationService(new LoggerMock()).Delete(id);
			Assert.Equal(DeleteResult.Deleted, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void Read_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var result = await new AuthenticationService(new LoggerMock()).Read(id);
			Assert.Equal(ReadResult.Read, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}

		[Fact]
		public async void Update_ShouldSucceed()
		{
			var id = Guid.NewGuid().ToString();
			var updateEntry = new UpdateUser
			{
				Id = id
			};

			var result = await new AuthenticationService(new LoggerMock()).Update(updateEntry);
			Assert.Equal(UpdateResult.Updated, result.Result);
			Assert.NotNull(result.Entry);
			Assert.Equal(id, result.Entry.Id);
		}
	}
}