namespace Service.Sdk.Contracts.Crud.User
{
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Describes the user data for the update operation of the <see cref="IUserService" />.
	/// </summary>
	public class UpdateUserEntry : BaseUserEntry, IEntry<string>
	{
		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}