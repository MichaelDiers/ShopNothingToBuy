namespace ProductsApi.Models
{
	using System;
	using Newtonsoft.Json;

	/// <summary>
	///   Defines a product used in data transfer context.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn, ItemRequired = Required.Always)]
	public class ProductDto
	{
		/// <summary>
		///   Creates a new instance of <see cref="ProductDto" />.
		/// </summary>
		public ProductDto()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="ProductDto" />.
		/// </summary>
		/// <param name="description">The description of the new product.</param>
		/// <param name="id">The id of the new product.</param>
		/// <param name="name">The name of the new product.</param>
		public ProductDto(string description, Guid id, string name)
		{
			this.Description = description;
			this.Id = id;
			this.Name = name;
		}

		/// <summary>
		///   Creates a new instance of <see cref="ProductDto" />.
		/// </summary>
		/// <param name="product">Defines the new values of the product.</param>
		public ProductDto(Product product)
			: this(product.Description, product.Id, product.Name)
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
		[JsonProperty(PropertyName = "id", Required = Required.Default)]
		public Guid Id { get; set; }

		/// <summary>
		///   Gets or sets a value used as the name of a product.
		/// </summary>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
	}
}