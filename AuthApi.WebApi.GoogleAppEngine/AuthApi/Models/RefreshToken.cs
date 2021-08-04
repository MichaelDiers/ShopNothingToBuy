namespace AuthApi.Models
{
	using Google.Cloud.Firestore;

	/// <summary>
	///   Describes a refresh token.
	/// </summary>
	[FirestoreData]
	public class RefreshToken
	{
		/// <summary>
		///   The entry name in the database for the refresh count.
		/// </summary>
		public const string DatabaseRefreshCountName = "refreshCount";

		/// <summary>
		///   The entry name in the database for the token.
		/// </summary>
		public const string DatabaseTokenName = "token";

		/// <summary>
		///   Gets or sets a counter that specifies how many times the refresh token chain is used. If the refresh token is used, a
		///   new token and refresh token is created and the used refresh token is invalidated. Each time a new refresh token is
		///   created the count is increased.
		/// </summary>
		[FirestoreProperty(DatabaseRefreshCountName)]
		public int RefreshCount { get; set; }

		/// <summary>
		///   Gets or sets the refresh token.
		/// </summary>
		[FirestoreProperty(DatabaseTokenName)]
		public string Token { get; set; }
	}
}