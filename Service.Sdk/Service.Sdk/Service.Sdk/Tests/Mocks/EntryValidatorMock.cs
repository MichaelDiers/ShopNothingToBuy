namespace Service.Sdk.Tests.Mocks
{
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Mock for generic entry validators.
	/// </summary>
	/// <typeparam name="TCreateEntry">The type for creating a new entry.</typeparam>
	/// <typeparam name="TUpdateEntry">The type for updating a new entry.</typeparam>
	/// <typeparam name="TEntryId">The type of the entry id.</typeparam>
	public class EntryValidatorMock<TCreateEntry, TUpdateEntry, TEntryId>
		: IEntryValidator<TCreateEntry, TUpdateEntry, TEntryId>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<TEntryId>
	{
		/// <summary>
		///   The result of <see cref="ValidateCreateEntry" />.
		/// </summary>
		private readonly bool validateCreateEntry;

		/// <summary>
		///   The result of <see cref="ValidateEntryId" />.
		/// </summary>
		private readonly bool validateEntryId;

		/// <summary>
		///   The result of <see cref="ValidateUpdateEntry" />.
		/// </summary>
		private readonly bool validateUpdateEntry;

		/// <summary>
		///   Creates new instance of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}" />.
		/// </summary>
		/// <param name="validateCreateEntry">The result of <see cref="ValidateCreateEntry" />.</param>
		/// <param name="validateEntryId">The result of <see cref="ValidateEntryId" />.</param>
		/// <param name="validateUpdateEntry">The result of <see cref="ValidateUpdateEntry" />.</param>
		public EntryValidatorMock(bool validateCreateEntry, bool validateEntryId, bool validateUpdateEntry)
		{
			this.validateCreateEntry = validateCreateEntry;
			this.validateEntryId = validateEntryId;
			this.validateUpdateEntry = validateUpdateEntry;
		}

		/// <summary>
		///   Creates new instance of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}" />.
		/// </summary>
		/// <param name="defaultResult">The result of the validator methods.</param>
		public EntryValidatorMock(bool defaultResult)
			: this(defaultResult, defaultResult, defaultResult)
		{
		}

		/// <summary>
		///   Creates new instance of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}" />. The default validator
		///   result is true.
		/// </summary>
		public EntryValidatorMock()
			: this(true, true, true)
		{
		}

		/// <summary>
		///   Gets or sets a value that is the count of <see cref="ValidateCreateEntry" /> calls.
		/// </summary>
		public int ValidateCreateEntryCallCount { get; private set; }

		/// <summary>
		///   Gets or sets a value that is the count of <see cref="ValidateEntryId" /> calls.
		/// </summary>
		public int ValidateEntryIdCallCount { get; private set; }

		/// <summary>
		///   Gets or sets a value that is the count of <see cref="ValidateUpdateEntryCallCount" /> calls.
		/// </summary>
		public int ValidateUpdateEntryCallCount { get; private set; }

		/// <summary>
		///   Validates a <typeparamref name="TCreateEntry" /> entry.
		/// </summary>
		/// <param name="entry">The entry to validate.</param>
		/// <returns>The value of <see cref="validateCreateEntry" />.</returns>
		public virtual Task<bool> ValidateCreateEntry(TCreateEntry entry)
		{
			this.ValidateCreateEntryCallCount++;
			return Task.FromResult(this.validateCreateEntry);
		}

		/// <summary>
		///   Validates a <typeparamref name="TEntryId" /> id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>The value of <see cref="validateEntryId" />.</returns>
		public virtual Task<bool> ValidateEntryId(TEntryId entryId)
		{
			this.ValidateEntryIdCallCount++;
			return Task.FromResult(this.validateEntryId);
		}

		/// <summary>
		///   Validates a <typeparamref name="TUpdateEntry" /> entry.
		/// </summary>
		/// <param name="entry">The entry to validate.</param>
		/// <returns>The value of <see cref="validateUpdateEntry" /></returns>
		/// .
		public virtual Task<bool> ValidateUpdateEntry(TUpdateEntry entry)
		{
			this.ValidateUpdateEntryCallCount++;
			return Task.FromResult(this.validateUpdateEntry);
		}
	}
}