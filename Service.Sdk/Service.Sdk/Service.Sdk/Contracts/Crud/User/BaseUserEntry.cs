﻿namespace Service.Sdk.Contracts.Crud.User
{
	using System.Collections.Generic;
	using Service.Sdk.Contracts.Crud.Application;

	/// <summary>
	///   User data that is used in <see cref="UserEntry" />, <see cref="CreateUserEntry" /> and
	///   <see cref="UpdateUserEntry" />
	///   .
	/// </summary>
	public abstract class BaseUserEntry
	{
		/// <summary>
		///   Creates a new instance of <see cref="BaseUserEntry" />.
		/// </summary>
		protected BaseUserEntry()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="BaseUserEntry" />.
		/// </summary>
		/// <param name="id">The id of the user.</param>
		/// <param name="applications">The applications and roles the user is allowed to access.</param>
		protected BaseUserEntry(string id, IEnumerable<UserApplicationEntry> applications)
		{
			this.Id = id;
			this.Applications = applications;
		}

		/// <summary>
		///   Gets or sets the id of the application the user is created for.
		/// </summary>
		public string ApplicationId { get; set; }

		/// <summary>
		///   Gets or sets the applications and roles that the user can access.
		/// </summary>
		public IEnumerable<UserApplicationEntry> Applications { get; set; }

		/// <summary>
		///   Gets or sets the user id.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///   Gets or sets the roles of the user.
		/// </summary>
		public Roles Roles { get; set; }
	}
}