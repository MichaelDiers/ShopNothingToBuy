namespace Service.Sdk.Tests
{
	using System;
	using Service.Contracts.Crud.Base;
	using Service.Sdk.Services;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="OperationResult{TEntry,TEntryId,TOperationResult}" />.
	/// </summary>
	public class OperationListResultTests
	{
		[Fact]
		public void OperationListResultWithResult_ShouldCreateInstance()
		{
			const ListResult result = ListResult.Completed;
			var operationListResult = new OperationListResult<string, ListResult>(result);
			Assert.NotNull(operationListResult);
			Assert.IsAssignableFrom<IOperationListResult<string, ListResult>>(operationListResult);
			Assert.Equal(result, operationListResult.Result);
			Assert.Null(operationListResult.Entries);
		}

		[Fact]
		public void OperationListResultWithResultAndEntry_ShouldCreateInstance()
		{
			const ListResult result = ListResult.Completed;
			var list = new[]
			{
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString(),
				Guid.NewGuid().ToString()
			};

			var operationListResult = new OperationListResult<string, ListResult>(
				result,
				list);
			Assert.NotNull(operationListResult);
			Assert.IsAssignableFrom<IOperationListResult<string, ListResult>>(operationListResult);
			Assert.Equal(result, operationListResult.Result);
			Assert.Equal(list, operationListResult.Entries);
		}
	}
}