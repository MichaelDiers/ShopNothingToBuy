namespace ProductsApi.Models
{
	using System;
	using Newtonsoft.Json;

	/// <summary>
	///   Defines a product.
	/// </summary>
	public class Product
	{
		/// <summary>
		///   Creates a new instance of <see cref="Product" />.
		/// </summary>
		public Product()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="Product" />.
		/// </summary>
		/// <param name="description">The description of the new product.</param>
		/// <param name="id">The id of the new product.</param>
		/// <param name="name">The name of the new product.</param>
		public Product(string description, Guid id, string name)
		{
			this.Description = description;
			this.Id = id;
			this.Name = name;
		}

		/// <summary>
		///   Creates a new instance of <see cref="Product" />.
		/// </summary>
		/// <param name="productDto">Defines the new values of the product.</param>
		public Product(ProductDto productDto)
			: this(productDto.Description, productDto.Id, productDto.Name)
		{
		}

		/// <summary>
		///   Gets or sets a value used as the description of a product.
		/// </summary>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		/// <summary>
		///   Gets or sets a value used as the id of a product.
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		/// <summary>
		///   Gets or sets a value used as the name of a product.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
	}
}