namespace AuthApi.Contracts
{
	/// <summary>
	///   Describes a role.
	/// </summary>
	public interface IRole
	{
		/// <summary>
		///   Gets the unique id of the role.
		/// </summary>
		string Id { get; }

		/// <summary>
		///   Gets the name of the role.
		/// </summary>
		string Name { get; }
	}
}