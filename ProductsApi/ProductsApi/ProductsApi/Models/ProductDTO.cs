namespace ProductsApi.Models
{
	using ProductsApi.Contracts;

	using System;

	public class ProductDTO : IProductDTO
	{
		public ProductDTO()
		{
		}

		public ProductDTO(IProduct product)
		{
			this.Description = product.Description;
			this.Id = product.Id;
			this.Name = product.Name;
		}

		public string Description { get; set; }
		public Guid Id { get; set; }
		public string Name { get; }
	}
}
