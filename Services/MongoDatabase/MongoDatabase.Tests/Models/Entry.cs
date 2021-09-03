namespace MongoDatabase.Tests.Models
{
	using Service.Sdk.Contracts;

	internal class Entry : IEntry<string>
	{
		public Entry(string id, string value)
		{
			this.Id = id;
			this.Value = value;
		}

		public string Value { get; set; }

		public string Id { get; set; }
	}
}