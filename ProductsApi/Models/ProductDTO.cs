namespace ProductsApi.Models
{
	using Newtonsoft.Json;

	using ProductsApi.Contracts;

	using System;

	/// <summary>
	/// Defines a product used in data transfer context.
	/// </summary>
	public class ProductDTO
	{
		/// <summary>
		/// Creates a new instance of <see cref="ProductDTO"/>.
		/// </summary>
		public ProductDTO()
		{
		}

		/// <summary>
		/// Creates a new instance of <see cref="ProductDTO"/>.
		/// </summary>
		/// <param name="description">The description of the new product.</param>
		/// <param name="id">The id of the new product.</param>
		/// <param name="name">The name of the new product.</param>
		public ProductDTO(string description, Guid id, string name)
		{
			Description = description;
			Id = id;
			Name = name;
		}

		/// <summary>
		/// Creates a new instance of <see cref="ProductDTO"/>.
		/// </summary>
		/// <param name="product">Defines the new values of the product.</param>
		public ProductDTO(Product product)
			: this(product.Description, product.Id, product.Name)
		{
		}

		/// <summary>
		/// Gets or sets a value used as the description of a product.
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a value used as the id of a product.
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets a value used as the name of a product.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
	}
}
