namespace ProductsApi.Contracts
{
	using Microsoft.AspNetCore.Http;

	/// <summary>
	///   Provides operations for handling api keys.
	/// </summary>
	public interface IApiKeyService
	{
		/// <summary>
		///   Check if the given guid is a valid api key.
		/// </summary>
		/// <param name="guid">The api key as a guid.</param>
		/// <returns>True if the api key is valid and otherwise false.</returns>
		bool IsValid(string guid);

		/// <summary>
		///   Extracts the api key from the given <see cref="HttpRequest" /> and checks if it is valid.
		/// </summary>
		/// <param name="httpRequest">A <see cref="HttpRequest" /> with a x-api-key header.</param>
		/// <returns>True if the api key is valid and otherwise false.</returns>
		bool IsValid(HttpRequest httpRequest);
	}
}