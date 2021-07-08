namespace ProductsApi.Contracts
{
	using System;

	public interface IProductDTO
	{
		string Description { get; }

		Guid Id { get; }

		string Name { get; }
	}
}
