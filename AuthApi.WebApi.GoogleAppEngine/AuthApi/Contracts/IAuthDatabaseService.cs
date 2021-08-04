namespace AuthApi.Contracts
{
	using System;
	using System.Threading.Tasks;

	/// <summary>
	///   Handle refresh tokens.
	/// </summary>
	public interface IAuthDatabaseService
	{
		/// <summary>
		///   Add a refresh token to the database.
		/// </summary>
		/// <param name="refreshToken">The token to be added.</param>
		/// <returns>True if the token is added and false otherwise.</returns>
		Task AddRefreshToken(string refreshToken);

		/// <summary>
		///   Delete all refresh tokens with a refresh count of more than <see paramref="maxRefreshCount" /> or that are expired
		///   according to <paramref name="validToValidation" />.
		/// </summary>
		/// <param name="maxRefreshCount">The maximum allowed refresh count for a refresh token.</param>
		/// <param name="validToValidation">Checks if a refresh token is already expired.</param>
		/// <returns>A <see cref="Task" />.</returns>
		Task RemoveInvalidRefreshTokens(int maxRefreshCount, Func<DateTime, bool> validToValidation);

		/// <summary>
		///   Replace an old refresh token by a new one.
		/// </summary>
		/// <param name="oldRefreshToken">This refresh token gets invalid and is removed from storage.</param>
		/// <param name="newRefreshToken">The new an valid token that is added to storage.</param>
		/// <returns>The current refresh count of the token or -1 if the operation failed.</returns>
		Task<int> ReplaceRefreshToken(string oldRefreshToken, string newRefreshToken);
	}
}