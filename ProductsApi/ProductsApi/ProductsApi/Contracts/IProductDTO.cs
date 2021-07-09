namespace ProductsApi.Contracts
{
	using System;

	/// <summary>
	/// Defines a product used in data transfer context.
	/// </summary>
	public interface IProductDTO
	{
		/// <summary>
		/// Gets a value used as the description of a product.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Gets a value used as the id of a product.
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// Gets a value used as the name of a product.
		/// </summary>
		string Name { get; }
	}
}
