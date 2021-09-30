namespace Service.Sdk.Tests
{
	using Service.Sdk.Contracts.Crud.Base;
	using Service.Sdk.Services;
	using Service.Sdk.Tests.Models;
	using Xunit;

	/// <summary>
	///   Tests for <see cref="OperationResult{TEntry,TEntryId,TOperationResult}" />.
	/// </summary>
	public class OperationResultTests
	{
		[Fact]
		public void OperationResultWithResult_ShouldCreateInstance()
		{
			const ClearResult result = ClearResult.Cleared;
			var operationResult = new OperationResult<StringEntry, string, ClearResult>(result);
			Assert.NotNull(operationResult);
			Assert.IsAssignableFrom<IOperationResult<StringEntry, string, ClearResult>>(operationResult);
			Assert.Equal(result, operationResult.Result);
			Assert.Null(operationResult.Entry);
		}

		[Fact]
		public void OperationResultWithResultAndEntry_ShouldCreateInstance()
		{
			const ClearResult result = ClearResult.Cleared;
			const int id = 47;
			var operationResult = new OperationResult<IntEntry, int, ClearResult>(
				result,
				new IntEntry
				{
					Id = id
				});
			Assert.NotNull(operationResult);
			Assert.IsAssignableFrom<IOperationResult<IntEntry, int, ClearResult>>(operationResult);
			Assert.Equal(result, operationResult.Result);
			Assert.Equal(id, operationResult.Entry.Id);
		}
	}
}