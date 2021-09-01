namespace User.Services.Models
{
	using Service.Sdk.Contracts;

	public class UpdateUserEntry : IEntry<string>
	{
		public string Id { get; set; }
	}
}