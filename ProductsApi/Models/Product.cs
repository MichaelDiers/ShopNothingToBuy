namespace ProductsApi.Models
{
	using ProductsApi.Contracts;

	using System;

	/// <summary>
	/// Defines a product.
	/// </summary>
	public class Product : IProduct
	{
		/// <summary>
		/// Creates a new instance of <see cref="Product"/>.
		/// </summary>
		public Product()
		{
		}

		/// <summary>
		/// Creates a new instance of <see cref="Product"/>.
		/// </summary>
		/// <param name="description">The description of the new product.</param>
		/// <param name="id">The id of the new product.</param>
		/// <param name="name">The name of the new product.</param>
		public Product(string description, Guid id, string name)
		{
			Description = description;
			Id = id;
			Name = name;
		}

		/// <summary>
		/// Creates a new instance of <see cref="Product"/>.
		/// </summary>
		/// <param name="productDTO">Defines the new values of the product.</param>
		public Product(IProductDTO productDTO)
			: this(productDTO.Description, productDTO.Id, productDTO.Name)
		{
		}

		/// <summary>
		/// Gets or sets a value used as the description of a product.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a value used as the id of a product.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets a value used as the name of a product.
		/// </summary>
		public string Name { get; set; }
	}
}
