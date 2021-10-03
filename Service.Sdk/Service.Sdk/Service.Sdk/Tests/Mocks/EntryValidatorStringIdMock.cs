namespace Service.Sdk.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using Service.Sdk.Contracts.Crud.Base;

	/// <summary>
	///   Mock for generic entry validators.
	/// </summary>
	/// <typeparam name="TCreateEntry">The type for creating a new entry.</typeparam>
	/// <typeparam name="TUpdateEntry">The type for updating a new entry.</typeparam>
	public class EntryValidatorStringIdMock<TCreateEntry, TUpdateEntry>
		: EntryValidatorMock<TCreateEntry, TUpdateEntry, string>
		where TCreateEntry : class
		where TUpdateEntry : class, IEntry<string>
	{
		/// <summary>
		///   Creates new instance of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}" />.
		/// </summary>
		/// <param name="validateCreateEntry">
		///   The result of
		///   <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}.ValidateCreateEntry" />.
		/// </param>
		/// <param name="validateEntryId">The result of <see cref="ValidateEntryId" />.</param>
		/// <param name="validateUpdateEntry">The result of <see cref="ValidateUpdateEntry" />.</param>
		public EntryValidatorStringIdMock(bool validateCreateEntry, bool validateEntryId, bool validateUpdateEntry)
			: base(validateCreateEntry, validateEntryId, validateUpdateEntry)
		{
		}

		/// <summary>
		///   Creates new instance of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}" />.
		/// </summary>
		/// <param name="defaultResult">The result of the validator methods.</param>
		public EntryValidatorStringIdMock(bool defaultResult)
			: this(defaultResult, defaultResult, defaultResult)
		{
		}

		/// <summary>
		///   Creates new instance of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}" />. The default validator
		///   result is true.
		/// </summary>
		public EntryValidatorStringIdMock()
			: this(true, true, true)
		{
		}

		/// <summary>
		///   Validates an id.
		/// </summary>
		/// <param name="entryId">The id of the entry.</param>
		/// <returns>The value of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}.ValidateEntryId" />.</returns>
		public override async Task<bool> ValidateEntryId(string entryId)
		{
			if (!string.IsNullOrWhiteSpace(entryId) && entryId != entryId.ToUpper())
			{
				throw new NotImplementedException();
			}

			return await base.ValidateEntryId(entryId);
		}

		/// <summary>
		///   Validates a <typeparamref name="TUpdateEntry" /> entry.
		/// </summary>
		/// <param name="entry">The entry to validate.</param>
		/// <returns>
		///   The value of <see cref="EntryValidatorMock{TCreateEntry,TUpdateEntry,TEntryId}.ValidateUpdateEntry" />
		/// </returns>
		/// .
		public override async Task<bool> ValidateUpdateEntry(TUpdateEntry entry)
		{
			if (entry != null && !string.IsNullOrWhiteSpace(entry.Id) && entry.Id != entry.Id.ToUpper())
			{
				throw new NotImplementedException();
			}

			return await base.ValidateUpdateEntry(entry);
		}
	}
}