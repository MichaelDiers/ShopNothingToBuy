namespace Service.GoogleCloud.Functions
{
	using System.Collections.Generic;

	/// <summary>
	///   Describes request data from a http context.
	/// </summary>
	public class RequestData : IRequestData
	{
		/// <summary>
		///   Gets or sets the api key from the request header.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		///   Gets or sets the request body.
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		///   Gets or sets the query data from the request.
		/// </summary>
		public IDictionary<string, string> Query { get; set; }

		/// <summary>
		///   Gets or sets the jwt from the request header.
		/// </summary>
		public string Token { get; set; }
	}
}