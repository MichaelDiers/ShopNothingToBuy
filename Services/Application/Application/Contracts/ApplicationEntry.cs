namespace Application.Contracts
{
	using System;
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes an application.
	/// </summary>
	public class ApplicationEntry : BaseApplicationEntry, IEntry<string>
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
		/// <param name="name">The name of the application.</param>
		public ApplicationEntry(string id, string name)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
			}

			this.Id = id;
			this.Name = name;
		}

		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="createApplicationEntry">Data is initialized from the given entry.</param>
		public ApplicationEntry(CreateApplicationEntry createApplicationEntry)
			: this(Guid.NewGuid().ToString(), createApplicationEntry.Name)
		{
		}

		/// <summary>
		///   Creates an instance of <see cref="ApplicationEntry" />.
		/// </summary>
		/// <param name="updateApplicationEntry">Data is initialized from the given entry.</param>
		public ApplicationEntry(UpdateApplicationEntry updateApplicationEntry)
			: this(updateApplicationEntry.Id, updateApplicationEntry.Name)
		{
		}

		/// <summary>
		///   Gets or sets the id of the application.
		/// </summary>
		public string Id { get; set; }
	}
}