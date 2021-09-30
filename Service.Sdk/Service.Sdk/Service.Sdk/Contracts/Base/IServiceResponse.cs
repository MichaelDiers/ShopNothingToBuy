namespace Service.Sdk.Contracts.Base
{
	using System;

	/// <summary>
	///   Describes the result of a service operation and the resulting object.
	/// </summary>
	/// <typeparam name="TOperationResult">The type of the result.</typeparam>
	/// <typeparam name="TResponse">The type of the resulting object.</typeparam>
	public interface IServiceResponse<out TOperationResult, out TResponse> : IServiceResult<TOperationResult>
		where TOperationResult : Enum
	{
		/// <summary>
		///   Gets the response of the operation that contains the result object.
		/// </summary>
		TResponse Response { get; }
	}
}