namespace Service.Contracts.Crud.Base
{
	/// <summary>
	///   Describes the result of a service operation, e.g.:
	///   <see cref="IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Create" />.
	/// </summary>
	/// <typeparam name="TEntry">The type of an entry that is processed..</typeparam>
	/// <typeparam name="TEntryId">The type of the of the processed entry.</typeparam>
	/// <typeparam name="TOperationResult">The type of the operation result, e.g.: <see cref="CreateResult" />.</typeparam>
	public interface IOperationResult<out TEntry, TEntryId, out TOperationResult> where TEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   Gets the processed entry of the service operation. The value is null if the service operation did not succeed.
		/// </summary>
		TEntry Entry { get; }

		/// <summary>
		///   Gets the result of the operation.
		/// </summary>
		/// <see cref="CreateResult" />
		/// .
		TOperationResult Result { get; }
	}
}