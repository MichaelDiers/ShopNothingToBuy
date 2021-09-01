namespace Authentication.Services.Models
{
	using Service.Sdk.Contracts;

	public class User : IEntry<string>
	{
		public User(string id)
		{
			this.Id = id;
		}

		public string Id { get; }
	}
}