namespace Service.Sdk.Contracts
{
	using System.Collections.Generic;

	/// <summary>
	///   Describes the result of a service list operation, e.g.:
	///   <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.List" />.
	/// </summary>
	/// <typeparam name="T">The type of the listed entries.</typeparam>
	/// <typeparam name="TOperationResult">The type of the operation list result, e.g.: <see cref="ListResult" />.</typeparam>
	public interface IOperationListResult<out T, out TOperationResult>
	{
		/// <summary>
		///   Gets the listed entries that are null if an error occurred.
		/// </summary>
		IEnumerable<T> Entries { get; }

		/// <summary>
		///   Gets the result of the list operation.
		/// </summary>
		/// <see cref="ListResult" />
		TOperationResult Result { get; }
	}
}