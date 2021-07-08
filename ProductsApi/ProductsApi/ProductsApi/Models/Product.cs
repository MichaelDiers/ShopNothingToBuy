namespace ProductsApi.Models
{
	using ProductsApi.Contracts;

	using System;

	public class Product : IProduct
	{
		public string Description { get; set; }
		public Guid Id { get; set; }
		public string Name { get; }
	}
}
