namespace Service.Sdk.Services
{
	using System.Collections.Generic;
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes the result of a service list operation, e.g.:
	///   <see cref="List{T}" />.
	/// </summary>
	/// <typeparam name="T">The type of the listed entries.</typeparam>
	/// <typeparam name="TOperationResult">The type of the operation list result, e.g.: <see cref="ListResult" />.</typeparam>
	public class OperationListResult<T, TOperationResult> : IOperationListResult<T, TOperationResult>
	{
		/// <summary>
		///   Creates a new instance of <see cref="OperationListResult{T,TOperationResult}" />.
		/// </summary>
		/// <param name="result">The result of the operation.</param>
		public OperationListResult(TOperationResult result)
			: this(result, null)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="OperationListResult{T,TOperationResult}" />.
		/// </summary>
		/// <param name="result">The result of the operation.</param>
		/// <param name="entries">The listed entries of the operation.</param>
		public OperationListResult(TOperationResult result, IEnumerable<T> entries)
		{
			this.Entries = entries;
			this.Result = result;
		}

		/// <summary>
		///   Gets the listed entries that are null if an error occurred.
		/// </summary>
		public IEnumerable<T> Entries { get; }

		/// <summary>
		///   Gets the result of the list operation.
		/// </summary>
		/// <see cref="ListResult" />
		public TOperationResult Result { get; }
	}
}