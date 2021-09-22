namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Contracts.Business.Log;
	using Service.Contracts.Crud.Base;
	using Service.Contracts.Crud.Database;
	using Service.Sdk.Contracts;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Models;

	internal class
		ServiceDatabaseBaseMock : ServiceDatabaseBase<StringEntry, IStringEntry, string, CreateEntry, UpdateEntry>
	{
		private readonly bool succeed;
		private readonly bool throwException;

		public ServiceDatabaseBaseMock(
			ILogger logger,
			IEntryValidator<CreateEntry, UpdateEntry, string> validator,
			IDatabaseService<StringEntry, string> databaseService,
			bool succeed,
			bool throwException)
			: base(logger, validator, databaseService)
		{
			this.succeed = succeed;
			this.throwException = throwException;
		}

		protected override Task<IOperationResult<IStringEntry, string, CreateResult>> CreateEntry(CreateEntry entry)
		{
			if (this.throwException)
			{
				throw new Exception(nameof(this.CreateEntry));
			}

			var result = this.succeed
				? new OperationResult<StringEntry, string, CreateResult>(CreateResult.Created, new StringEntry(entry))
				: new OperationResult<StringEntry, string, CreateResult>(CreateResult.AlreadyExists);
			return Task.FromResult<IOperationResult<IStringEntry, string, CreateResult>>(result);
		}

		protected override Task<IOperationResult<IStringEntry, string, UpdateResult>> UpdateEntry(UpdateEntry entry)
		{
			if (this.throwException)
			{
				throw new Exception(nameof(this.UpdateEntry));
			}

			var result = this.succeed
				? new OperationResult<StringEntry, string, UpdateResult>(UpdateResult.Updated, new StringEntry(entry))
				: new OperationResult<StringEntry, string, UpdateResult>(UpdateResult.NotFound);
			return Task.FromResult<IOperationResult<IStringEntry, string, UpdateResult>>(result);
		}
	}
}