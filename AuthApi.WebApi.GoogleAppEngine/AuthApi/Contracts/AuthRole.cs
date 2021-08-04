namespace AuthApi.Contracts
{
	using System;

	/// <summary>
	///   Defines the roles of the application.
	/// </summary>
	[Flags]
	public enum AuthRole
	{
		/// <summary>
		///   Undefined role.
		/// </summary>
		None = 0,

		/// <summary>
		///   The user role.
		/// </summary>
		User = 1 << 0,

		/// <summary>
		///   A Readonly role.
		/// </summary>
		Reader = 1 << 1,

		/// <summary>
		///   Role allows reading and writing data.
		/// </summary>
		Writer = 1 << 2,

		/// <summary>
		///   Role allows access to all application functions.
		/// </summary>
		Admin = 1 << 3,

		/// <summary>
		///   All roles.
		/// </summary>
		All = User | Reader | Writer | Admin
	}
}