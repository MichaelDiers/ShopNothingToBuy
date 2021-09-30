namespace Service.Sdk.Models.Base
{
	using System;
	using Service.Sdk.Contracts.Base;

	/// <summary>
	///   Describes the result of a service operation and the resulting object.
	/// </summary>
	/// <typeparam name="TOperationResult">The type of the result.</typeparam>
	/// <typeparam name="TResponse">The type of the resulting object.</typeparam>
	public class ServiceResponse<TOperationResult, TResponse> : ServiceResult<TOperationResult>,
		IServiceResponse<TOperationResult, TResponse> where TOperationResult : Enum
	{
		/// <summary>
		///   Gets the response of the operation that contains the result object.
		/// </summary>
		public TResponse Response { get; set; }
	}
}