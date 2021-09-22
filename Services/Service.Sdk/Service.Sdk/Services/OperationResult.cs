namespace Service.Sdk.Services
{
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes the result of a service operation.
	/// </summary>
	/// <typeparam name="TEntry">The type of an entry.</typeparam>
	/// <typeparam name="TEntryId">The type of the id of an entry.</typeparam>
	/// <typeparam name="TOperationResult">
	///   The type of an operation result, e.g. <see cref="CreateResult" /> or
	///   <see cref="DeleteResult" />.
	/// </typeparam>
	public class OperationResult<TEntry, TEntryId, TOperationResult>
		: IOperationResult<TEntry, TEntryId, TOperationResult> where TEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Creates a new instance of <see cref="OperationResult{TEntry,TEntryId,TOperationResult}" />.
		/// </summary>
		/// <param name="operationResult">The result of the operation.</param>
		public OperationResult(TOperationResult operationResult)
			: this(operationResult, null)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="OperationResult{TEntry,TEntryId,TOperationResult}" />.
		/// </summary>
		/// <param name="operationResult">The result of the operation.</param>
		/// <param name="entry">The associated entry of the operation.</param>
		public OperationResult(TOperationResult operationResult, TEntry entry)
		{
			this.Result = operationResult;
			this.Entry = entry;
		}

		/// <summary>
		///   Gets the entry that is associated with the operation.
		/// </summary>
		public TEntry Entry { get; }

		/// <summary>
		///   Gets the operation result, e.g. <see cref="CreateResult" /> or <see cref="DeleteResult" />.
		/// </summary>
		public TOperationResult Result { get; }
	}
}