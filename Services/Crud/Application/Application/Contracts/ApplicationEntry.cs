﻿namespace Application.Contracts
{
	using Service.Contracts.Crud.Application;
	using Service.Contracts.Crud.Base;

	/// <summary>
	///   Describes an application.
	/// </summary>
	public class ApplicationEntry : BaseApplicationEntry, IApplicationEntry, IEntry<string>
	{
		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		public ApplicationEntry()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="id">The id of the application.</param>
		/// <param name="originalId">The original requested id at creation time.</param>
		/// <param name="roles">The available roles of the application.</param>
		public ApplicationEntry(string id, string originalId, Roles roles)
			: base(id, roles)
		{
			this.OriginalId = originalId;
		}

		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="createApplicationEntry">Data is initialized from the given entry.</param>
		public ApplicationEntry(ICreateApplicationEntry createApplicationEntry)
			: this(
				createApplicationEntry?.Id?.ToUpper(),
				createApplicationEntry?.Id,
				createApplicationEntry?.Roles ?? Roles.None)
		{
		}

		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="updateApplicationEntry">Data is initialized from the given entry.</param>
		public ApplicationEntry(IUpdateApplicationEntry updateApplicationEntry)
			: this(
				updateApplicationEntry?.Id?.ToUpper(),
				updateApplicationEntry?.OriginalId,
				updateApplicationEntry?.Roles ?? Roles.None)
		{
		}

		/// <summary>
		///   Gets or sets the original requested id at creation time.
		/// </summary>
		public string OriginalId { get; set; }
	}
}