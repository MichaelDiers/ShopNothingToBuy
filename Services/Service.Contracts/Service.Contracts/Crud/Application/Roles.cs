namespace Service.Contracts.Crud.Application
{
	using System;

	/// <summary>
	///   Specifies the roles of an application.
	/// </summary>
	[Flags]
	public enum Roles
	{
		/// <summary>
		///   Undefined role.
		/// </summary>
		None = 0,

		/// <summary>
		///   Role for reading data.
		/// </summary>
		/// <see cref="Base.IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Exists" />
		/// <see cref="Base.IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Read" />
		Reader = 1 << 0,

		/// <summary>
		///   Role for writing data.
		/// </summary>
		/// <see cref="Base.IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Create" />
		/// <see cref="Base.IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Delete" />
		/// <see cref="Base.IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Update" />
		Writer = 1 << 1,

		/// <summary>
		///   Admin role.
		/// </summary>
		/// <see cref="Base.IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.Clear" />
		/// <see cref="Base.IServiceBase{TEntry,TEntryId,TCreateEntry,TUpdateEntry}.List" />
		Admin = 1 << 2,

		/// <summary>
		///   Role for reading and writing data.
		/// </summary>
		/// <see cref="Reader" />
		/// <see cref="Writer" />
		ReaderWriter = Reader | Writer,

		/// <summary>
		///   Combination of all available roles.
		/// </summary>
		/// <see cref="Reader" />
		/// <see cref="Writer" />
		/// <see cref="Admin" />
		All = Reader | Writer | Admin
	}
}