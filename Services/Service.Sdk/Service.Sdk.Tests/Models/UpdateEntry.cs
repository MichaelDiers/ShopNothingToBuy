namespace Service.Sdk.Tests.Models
{
	using Service.Sdk.Contracts.Crud.Base;

	public class UpdateEntry : IEntry<string>
	{
		public UpdateEntry(string id, string toBeUpdated)
		{
			this.ToBeUpdated = toBeUpdated;
			this.Id = id;
		}

		public string ToBeUpdated { get; }
		public string Id { get; }
	}
}