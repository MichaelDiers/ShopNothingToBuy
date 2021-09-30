namespace Service.Sdk.Models.Base
{
	using System;
	using Service.Sdk.Contracts.Base;

	/// <summary>
	///   Describes the result of a service operation.
	/// </summary>
	/// <typeparam name="TOperationResult">The type of the result.</typeparam>
	public class ServiceResult<TOperationResult> : IServiceResult<TOperationResult> where TOperationResult : Enum
	{
		/// <summary>
		///   Gets the result of the operation that indicates success or failure.
		/// </summary>
		public TOperationResult Result { get; set; }
	}
}