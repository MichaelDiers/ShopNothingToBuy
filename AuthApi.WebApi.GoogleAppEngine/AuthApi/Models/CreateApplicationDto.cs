namespace AuthApi.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///   Data-transfer object for an application to be created.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CreateApplicationDto<T> where T : class
	{
		/// <summary>
		///   Gets or sets the name of the application.
		/// </summary>
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Name { get; set; }

		/// <summary>
		///   Gets or sets the supported roles of the application.
		/// </summary>
		[Required]
		[MinLength(1)]
		[MaxLength(10)]
		public IEnumerable<T> Roles { get; set; }
	}
}