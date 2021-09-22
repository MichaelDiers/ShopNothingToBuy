namespace Service.Sdk.Tests.Models
{
	using System;
	using Service.Contracts.Crud.Base;

	internal class StringEntry : IEntry<string>, IStringEntry
	{
		public StringEntry(CreateEntry entry)
			: this(Guid.NewGuid().ToString(), entry.Value.ToString())
		{
		}

		public StringEntry(UpdateEntry entry)
			: this(entry.Id, entry.ToBeUpdated)
		{
		}

		public StringEntry(string id, string value)
		{
			this.Id = id;
			this.Value = value;
		}

		public string Id { get; }

		public string Value { get; }
	}
}