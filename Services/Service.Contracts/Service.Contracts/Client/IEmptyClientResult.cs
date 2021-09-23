namespace Service.Contracts.Client
{
	using System;
	using System.Net;

	/// <summary>
	///   The result of a service call that does not include body data.
	/// </summary>
	public interface IEmptyClientResult
	{
		/// <summary>
		///   Gets the <see cref="Exception" /> if occurred at the service call.
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		///   Gets the resulting status code of the service call.
		/// </summary>
		public HttpStatusCode StatusCode { get; }
	}
}