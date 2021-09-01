namespace Authentication.Services.Models
{
	using Service.Sdk.Contracts;

	public class UpdateUser : IEntry<string>
	{
		public string Id { get; set; }
	}
}