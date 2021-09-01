namespace Service.Sdk.Tests.Models
{
	using Service.Sdk.Contracts;

	internal class IntEntry : IEntry<int>
	{
		public int Id { get; set; }
	}
}