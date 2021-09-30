namespace Service.Sdk.Clients
{
	using System;
	using System.Net;
	using Service.Sdk.Contracts.Client;

	/// <summary>
	///   The result of a service call that does not include body data.
	/// </summary>
	public class EmptyClientResult : IEmptyClientResult
	{
		/// <summary>
		///   Gets or sets the <see cref="Exception" /> if occurred at the service call.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		///   Gets or sets the resulting status code of the service call.
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }
	}
}