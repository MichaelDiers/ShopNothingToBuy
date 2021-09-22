namespace Service.Contracts.Base
{
	using System;

	/// <summary>
	///   Describes the result of a service operation.
	/// </summary>
	/// <typeparam name="TOperationResult">The type of the result.</typeparam>
	public interface IServiceResult<out TOperationResult> where TOperationResult : Enum
	{
		/// <summary>
		///   Gets the result of the operation that indicates success or failure.
		/// </summary>
		TOperationResult Result { get; }
	}
}