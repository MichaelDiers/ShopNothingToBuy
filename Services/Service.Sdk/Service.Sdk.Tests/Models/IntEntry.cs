namespace Service.Sdk.Tests.Models
{
	using Service.Contracts.Crud.Base;

	internal class IntEntry : IEntry<int>
	{
		public int Id { get; set; }
	}
}