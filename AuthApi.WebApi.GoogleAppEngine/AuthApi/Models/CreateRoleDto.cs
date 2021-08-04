namespace AuthApi.Models
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///   Data-transfer object for role to be created.
	/// </summary>
	public class CreateRoleDto
	{
		/// <summary>
		///   Gets or sets the name of the role.
		/// </summary>
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Name { get; set; }
	}
}