namespace Service.Sdk.Tests.Models
{
	using Service.Sdk.Contracts.Crud.Base;

	internal class IntEntry : IEntry<int>
	{
		public int Id { get; set; }
	}
}