namespace User.Services.Models
{
	using Service.Sdk.Contracts;

	public class UserEntry : IEntry<string>
	{
		public UserEntry(string id)
		{
			this.Id = id;
		}

		public string Id { get; }
	}
}