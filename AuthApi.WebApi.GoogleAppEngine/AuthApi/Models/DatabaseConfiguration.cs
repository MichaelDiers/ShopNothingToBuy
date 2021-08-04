namespace AuthApi.Models
{
	/// <summary>
	///   Describes the database configuration.
	/// </summary>
	public class DatabaseConfiguration
	{
		/// <summary>
		///   Gets or sets the database collection name for accounts.
		/// </summary>
		public string CollectionNameAccounts { get; set; }

		/// <summary>
		///   Gets or sets the database collection name for applications.
		/// </summary>
		public string CollectionNameApplications { get; set; }

		/// <summary>
		///   Gets or sets the database collection name for valid refresh tokens.
		/// </summary>
		public string CollectionNameRefreshTokens { get; set; }

		/// <summary>
		///   Gets or sets the name of the google cloud project in that the database is hosted.
		/// </summary>
		public string GoogleProject { get; set; }

		/// <summary>
		///   Check if the configuration is valid.
		/// </summary>
		/// <returns>True if all values are set and false otherwise.</returns>
		public bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(this.CollectionNameAccounts)
			       && !string.IsNullOrWhiteSpace(this.CollectionNameApplications)
			       && !string.IsNullOrWhiteSpace(this.CollectionNameRefreshTokens)
			       && !string.IsNullOrWhiteSpace(this.GoogleProject);
		}
	}
}