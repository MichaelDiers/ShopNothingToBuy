namespace ProductsApi.Contracts
{
	using System;

	public interface IProduct
	{
		string Description { get; }

		Guid Id { get; }

		string Name { get; }
	}
}
