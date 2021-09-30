namespace Service.Sdk.Tests
{
	using System;
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Mocks;
	using Service.Sdk.Tests.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="ServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}" />.
	/// </summary>
	public class ServiceBaseTests
	{
		[Fact]
		public async void Clear_ShouldFailIfServiceAndLoggerThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorLoggerMock());

			Assert.Equal(ClearResult.InternalError, await service.Clear());

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing clear entries.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Clear), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Clear_ShouldFailIfServiceFails()
		{
			var (logger, validator, service) = InitOperationFailServiceMock();

			Assert.Equal(ClearResult.InternalError, await service.Clear());

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Clear_ShouldFailIfServiceThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock();

			Assert.Equal(ClearResult.InternalError, await service.Clear());

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing clear entries.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Clear), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Clear_ShouldSucceed()
		{
			var (logger, validator, service) = InitServiceMock();

			Assert.Equal(ClearResult.Cleared, await service.Clear());

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.Exception);
			Assert.Null(logger.ErrorMessage);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Create_ShouldFailIfAlreadyExists()
		{
			var (logger, validator, service) = InitOperationFailServiceMock();
			const int value = 10;
			var createEntry = new CreateEntry(value);

			var result = await service.Create(createEntry);

			Assert.Equal(CreateResult.AlreadyExists, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(1, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Create_ShouldFailIfCreateEntryIsInvalid()
		{
			var (logger, validator, service) = InitServiceMock(new ValidatorMock(false, true, true));
			const int value = 10;
			var createEntry = new CreateEntry(value);

			var result = await service.Create(createEntry);

			Assert.Equal(CreateResult.InvalidData, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.Exception);
			Assert.Null(logger.ErrorMessage);

			Assert.Equal(1, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Create_ShouldFailIfCreateEntryThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock();
			const int value = 10;
			var createEntry = new CreateEntry(value);

			var result = await service.Create(createEntry);

			Assert.Equal(CreateResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing create entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Create), logger.Exception.Message);

			Assert.Equal(1, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Create_ShouldFailIfServiceAndLoggerThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorLoggerMock());
			const int value = 10;
			var createEntry = new CreateEntry(value);

			var result = await service.Create(createEntry);

			Assert.Equal(CreateResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing create entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Create), logger.Exception.Message);

			Assert.Equal(1, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Create_ShouldFailIfValidatorThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorValidatorMock());
			const int value = 10;
			var createEntry = new CreateEntry(value);

			var result = await service.Create(createEntry);

			Assert.Equal(CreateResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing create entry.", logger.ErrorMessage);
			Assert.Equal(nameof(validator.ValidateCreateEntry), logger.Exception.Message);

			Assert.Equal(1, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}


		[Fact]
		public async void Create_ShouldSucceed()
		{
			var (logger, validator, service) = InitServiceMock();
			const int value = 10;
			var createEntry = new CreateEntry(value);

			var result = await service.Create(createEntry);

			Assert.Equal(CreateResult.Created, result.Result);
			Assert.Equal(value.ToString(), result.Entry.Value);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(1, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Delete_ShouldFailIfIdIsInvalid()
		{
			var (logger, validator, service) = InitServiceMock(new ValidatorMock(true, false, true));
			const string id = "10";

			var result = await service.Delete(id);

			Assert.Equal(DeleteResult.InvalidData, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Delete_ShouldFailIfNotFound()
		{
			var (logger, validator, service) = InitOperationFailServiceMock();
			const string id = "10";

			var result = await service.Delete(id);

			Assert.Equal(DeleteResult.NotFound, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Delete_ShouldFailIfServiceAndLoggerThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorLoggerMock());
			const string id = "10";

			var result = await service.Delete(id);

			Assert.Equal(DeleteResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing delete entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Delete), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Delete_ShouldFailIfServiceThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock();
			const string id = "10";

			var result = await service.Delete(id);

			Assert.Equal(DeleteResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing delete entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Delete), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Delete_ShouldFailIfValidatorThrowsAnException()
		{
			var (logger, validator, service) = InitServiceMock(new ErrorValidatorMock());
			const string id = "10";

			var result = await service.Delete(id);

			Assert.Equal(DeleteResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing delete entry.", logger.ErrorMessage);
			Assert.Equal(nameof(validator.ValidateEntryId), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Delete_ShouldSucceed()
		{
			var (logger, validator, service) = InitServiceMock();
			const string id = "10";

			var result = await service.Delete(id);

			Assert.Equal(DeleteResult.Deleted, result.Result);
			Assert.NotNull(result.Entry.Value);
			Assert.Equal(id, result.Entry.Id);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		// 

		[Fact]
		public async void Exists_ShouldFailIfIdIsInvalid()
		{
			var (logger, validator, service) = InitServiceMock(new ValidatorMock(true, false, true));
			const string id = "10";

			var result = await service.Exists(id);

			Assert.Equal(ExistsResult.InvalidData, result);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Exists_ShouldFailIfNotFound()
		{
			var (logger, validator, service) = InitOperationFailServiceMock();
			const string id = "10";

			var result = await service.Exists(id);

			Assert.Equal(ExistsResult.NotFound, result);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Exists_ShouldFailIfServiceAndLoggerThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorLoggerMock());
			const string id = "10";

			var result = await service.Exists(id);

			Assert.Equal(ExistsResult.InternalError, result);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing exists entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Exists), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Exists_ShouldFailIfServiceThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock();
			const string id = "10";

			var result = await service.Exists(id);

			Assert.Equal(ExistsResult.InternalError, result);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing exists entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Exists), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Exists_ShouldFailIfValidatorThrowsAnException()
		{
			var (logger, validator, service) = InitServiceMock(new ErrorValidatorMock());
			const string id = "10";

			var result = await service.Exists(id);

			Assert.Equal(ExistsResult.InternalError, result);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing exists entry.", logger.ErrorMessage);
			Assert.Equal(nameof(validator.ValidateEntryId), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Exists_ShouldSucceed()
		{
			var (logger, validator, service) = InitServiceMock();
			const string id = "10";

			var result = await service.Exists(id);

			Assert.Equal(ExistsResult.Exists, result);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void List_ShouldFailIfServiceAndLoggerThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorLoggerMock());

			var result = await service.List();
			Assert.Equal(ListResult.InternalError, result.Result);
			Assert.Null(result.Entries);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing list.", logger.ErrorMessage);
			Assert.Equal(nameof(service.List), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void List_ShouldFailIfServiceFails()
		{
			var (logger, validator, service) = InitOperationFailServiceMock();

			var result = await service.List();
			Assert.Equal(ListResult.InternalError, result.Result);
			Assert.Null(result.Entries);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void List_ShouldFailIfServiceThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock();

			var result = await service.List();
			Assert.Equal(ListResult.InternalError, result.Result);
			Assert.Null(result.Entries);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing list.", logger.ErrorMessage);
			Assert.Equal(nameof(service.List), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void List_ShouldSucceed()
		{
			var (logger, validator, service) = InitServiceMock();

			var result = await service.List();
			Assert.Equal(ListResult.Completed, result.Result);
			Assert.NotNull(result.Entries);
			Assert.NotEmpty(result.Entries);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.Exception);
			Assert.Null(logger.ErrorMessage);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Read_ShouldFailIfIdIsInvalid()
		{
			var (logger, validator, service) = InitServiceMock(new ValidatorMock(true, false, true));
			const string id = "10";

			var result = await service.Read(id);

			Assert.Equal(ReadResult.InvalidData, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Read_ShouldFailIfNotFound()
		{
			var (logger, validator, service) = InitOperationFailServiceMock();
			const string id = "10";

			var result = await service.Read(id);

			Assert.Equal(ReadResult.NotFound, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Read_ShouldFailIfServiceAndLoggerThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorLoggerMock());
			const string id = "10";

			var result = await service.Read(id);

			Assert.Equal(ReadResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing read entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Read), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Read_ShouldFailIfServiceThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock();
			const string id = "10";

			var result = await service.Read(id);

			Assert.Equal(ReadResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing read entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Read), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Read_ShouldFailIfValidatorThrowsAnException()
		{
			var (logger, validator, service) = InitServiceMock(new ErrorValidatorMock());
			const string id = "10";

			var result = await service.Read(id);

			Assert.Equal(ReadResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing read entry.", logger.ErrorMessage);
			Assert.Equal(nameof(validator.ValidateEntryId), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Read_ShouldSucceed()
		{
			var (logger, validator, service) = InitServiceMock();
			const string id = "10";

			var result = await service.Read(id);

			Assert.Equal(ReadResult.Read, result.Result);
			Assert.NotNull(result.Entry.Value);
			Assert.Equal(id, result.Entry.Id);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(0, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(1, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public void ServiceBase_ShouldThrowExceptionIfLoggerIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new ServiceMock(null, new ValidatorMock()));
		}

		[Fact]
		public void ServiceBase_ShouldThrowExceptionIfValidatorIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new ServiceMock(new LoggerMock(), null));
		}

		[Fact]
		public async void Update_ShouldFailIfNotFound()
		{
			var (logger, validator, service) = InitOperationFailServiceMock();
			const string id = "10";
			const string value = "value";
			var updateEntry = new UpdateEntry(id, value);

			var result = await service.Update(updateEntry);

			Assert.Equal(UpdateResult.NotFound, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.Exception);
			Assert.Null(logger.ErrorMessage);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(1, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Update_ShouldFailIfServiceAndLoggerThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorLoggerMock());
			const string id = "10";
			const string value = "value";
			var updateEntry = new UpdateEntry(id, value);

			var result = await service.Update(updateEntry);

			Assert.Equal(UpdateResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing update entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Update), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(1, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Update_ShouldFailIfUpdateEntryIsInvalid()
		{
			var (logger, validator, service) = InitServiceMock(new ValidatorMock(true, true, false));
			const string id = "10";
			const string value = "value";
			var updateEntry = new UpdateEntry(id, value);

			var result = await service.Update(updateEntry);

			Assert.Equal(UpdateResult.InvalidData, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.Exception);
			Assert.Null(logger.ErrorMessage);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(1, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Update_ShouldFailIfUpdateEntryThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock();
			const string id = "10";
			const string value = "value";
			var updateEntry = new UpdateEntry(id, value);

			var result = await service.Update(updateEntry);

			Assert.Equal(UpdateResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing update entry.", logger.ErrorMessage);
			Assert.Equal(nameof(service.Update), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(1, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		[Fact]
		public async void Update_ShouldFailIfValidatorThrowsAnException()
		{
			var (logger, validator, service) = InitErrorServiceMock(new ErrorValidatorMock());
			const string id = "10";
			const string value = "value";
			var updateEntry = new UpdateEntry(id, value);

			var result = await service.Update(updateEntry);

			Assert.Equal(UpdateResult.InternalError, result.Result);
			Assert.Null(result.Entry);

			Assert.Equal(1, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Equal("Error executing update entry.", logger.ErrorMessage);
			Assert.Equal(nameof(validator.ValidateUpdateEntry), logger.Exception.Message);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(1, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}


		[Fact]
		public async void Update_ShouldSucceed()
		{
			var (logger, validator, service) = InitServiceMock();
			const string id = "10";
			const string value = "value";
			var updateEntry = new UpdateEntry(id, value);

			var result = await service.Update(updateEntry);

			Assert.Equal(UpdateResult.Updated, result.Result);
			Assert.Equal(value, result.Entry.Value);
			Assert.Equal(id, result.Entry.Id);

			Assert.Equal(0, logger.ErrorMessageAndExceptionCallCount);
			Assert.Equal(0, logger.ErrorMessageCallCount);
			Assert.Null(logger.ErrorMessage);
			Assert.Null(logger.Exception);

			Assert.Equal(0, validator.ValidateCreateEntryCallCount);
			Assert.Equal(1, validator.ValidateUpdateEntryCallCount);
			Assert.Equal(0, validator.ValidateEntryIdCallCount);
		}

		private static (IExtendedLogger, IExtendedEntryValidator,
			IServiceBase<StringEntry, string, CreateEntry, UpdateEntry>)
			InitErrorServiceMock()
		{
			var logger = new LoggerMock();
			var validator = new ValidatorMock(true, true, true);
			var service = new ErrorServiceMock(logger, validator);
			return (logger, validator, service);
		}

		private static (IExtendedLogger, IExtendedEntryValidator,
			IServiceBase<StringEntry, string, CreateEntry, UpdateEntry>)
			InitErrorServiceMock(IExtendedEntryValidator validator)
		{
			var logger = new LoggerMock();
			var service = new ErrorServiceMock(logger, validator);
			return (logger, validator, service);
		}

		private static (IExtendedLogger, IExtendedEntryValidator,
			IServiceBase<StringEntry, string, CreateEntry, UpdateEntry>)
			InitErrorServiceMock(IExtendedLogger logger)
		{
			var validator = new ValidatorMock(true, true, true);
			var service = new ErrorServiceMock(logger, validator);
			return (logger, validator, service);
		}

		private static (IExtendedLogger, IExtendedEntryValidator,
			IServiceBase<StringEntry, string, CreateEntry, UpdateEntry>)
			InitOperationFailServiceMock()
		{
			var logger = new LoggerMock();
			var validatorMock = new ValidatorMock();
			var service = new OperationFailServiceMock(logger, validatorMock);
			return (logger, validatorMock, service);
		}

		private static (IExtendedLogger, IExtendedEntryValidator,
			IServiceBase<StringEntry, string, CreateEntry, UpdateEntry>)
			InitServiceMock()
		{
			var logger = new LoggerMock();
			var validator = new ValidatorMock(true, true, true);
			var service = new ServiceMock(logger, validator);
			return (logger, validator, service);
		}

		private static (IExtendedLogger, IExtendedEntryValidator,
			IServiceBase<StringEntry, string, CreateEntry, UpdateEntry>)
			InitServiceMock(IExtendedEntryValidator validatorMock)
		{
			var logger = new LoggerMock();
			var service = new ServiceMock(logger, validatorMock);
			return (logger, validatorMock, service);
		}
	}
}