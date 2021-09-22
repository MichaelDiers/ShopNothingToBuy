namespace Service.Sdk.Tests.Models
{
	using Service.Contracts.Crud.Base;

	internal interface IStringEntry : IEntry<string>
	{
		string Value { get; }
	}
}