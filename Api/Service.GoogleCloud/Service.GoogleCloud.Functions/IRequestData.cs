namespace Service.GoogleCloud.Functions
{
	using System.Collections.Generic;

	/// <summary>
	///   Describes request data from a http context.
	/// </summary>
	public interface IRequestData
	{
		/// <summary>
		///   Gets the api key from the request header.
		/// </summary>
		string ApiKey { get; }

		/// <summary>
		///   Gets the request body.
		/// </summary>
		string Body { get; }

		/// <summary>
		///   Gets the query data from the request.
		/// </summary>
		IDictionary<string, string> Query { get; }

		/// <summary>
		///   Gets the jwt from the request header.
		/// </summary>
		string Token { get; }
	}
}