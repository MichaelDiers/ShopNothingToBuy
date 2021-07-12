namespace ProductsApi.Services
{
	using System;
	using Microsoft.AspNetCore.Http;
	using ProductsApi.Contracts;

	/// <summary>
	///   Provides operations for handling api keys.
	/// </summary>
	public class ApiKeyService : IApiKeyService
	{
		/// <summary>
		///   Name of the request header that contains the api key.
		/// </summary>
		private const string headerName = "x-api-key";

		/// <summary>
		///   A valid api key.
		/// </summary>
		private readonly Guid apiKey;

		/// <summary>
		///   Creates a new instance of <see cref="ApiKeyService" />.
		/// </summary>
		/// <param name="apiKey">A valid api key.</param>
		public ApiKeyService(Guid apiKey)
		{
			if (apiKey == Guid.Empty)
			{
				throw new ArgumentException("Api-key equals Guid.Empty", nameof(apiKey));
			}

			this.apiKey = apiKey;
		}

		/// <summary>
		///   Creates a new instance of <see cref="ApiKeyService" />.
		/// </summary>
		/// <param name="apiKey">A valid api key.</param>
		public ApiKeyService(string apiKey)
			: this(new Guid(apiKey))
		{
		}

		/// <summary>
		///   Check if the given guid is a valid api key.
		/// </summary>
		/// <param name="guid">The api key as a guid.</param>
		/// <returns>True if the api key is valid and otherwise false.</returns>
		public bool IsValid(string guid)
		{
			if (Guid.TryParse(guid, out var parsedApiKey))
			{
				return parsedApiKey == this.apiKey;
			}

			return false;
		}

		/// <summary>
		///   Extracts the api key from the given <see cref="HttpRequest" /> and checks if it is valid.
		/// </summary>
		/// <param name="httpRequest">A <see cref="HttpRequest" /> with a x-api-key header.</param>
		/// <returns>True if the api key is valid and otherwise false.</returns>
		public bool IsValid(HttpRequest httpRequest)
		{
			return httpRequest.Headers.ContainsKey(headerName) && this.IsValid(httpRequest.Headers[headerName]);
		}
	}
}