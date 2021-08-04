namespace AuthApi.Models
{
	using System.ComponentModel.DataAnnotations;
	using AuthApi.Contracts;
	using ShopNothingToBuy.Sdk.Attributes;

	/// <summary>
	///   Data-transfer object for a role.
	/// </summary>
	public class RoleDto
	{
		/// <summary>
		///   Creates a new instance of <see cref="RoleDto" />.
		/// </summary>
		public RoleDto()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="RoleDto" />.
		/// </summary>
		/// <param name="role">Data is initialized from the data of the given role.</param>
		public RoleDto(IRole role)
		{
			this.Id = role.Id;
			this.Name = role.Name;
		}

		/// <summary>
		///   Gets or sets the id of the role.
		/// </summary>
		[Required]
		[IsGuid(false, false)]
		public string Id { get; set; }

		/// <summary>
		///   Gets or sets the name of the role.
		/// </summary>
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Name { get; set; }
	}
}