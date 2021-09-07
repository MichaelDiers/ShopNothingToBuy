namespace User.Services.Models
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes the user data for the update operation of the <see cref="UserService" />.
	/// </summary>
	public class UpdateUserEntry : BaseUser, IEntry<string>
	{
		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}