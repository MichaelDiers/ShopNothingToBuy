namespace AuthApi.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.Security.Claims;
	using ShopNothingToBuy.Sdk.Attributes;

	/// <summary>
	///   Data-transfer object for a claim.
	/// </summary>
	public class ClaimDto
	{
		/// <summary>
		///   Creates a new instance of <see cref="ClaimDto" />.
		/// </summary>
		public ClaimDto()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="ClaimDto" />.
		/// </summary>
		/// <param name="claim">The claim is initialized by the given data.</param>
		public ClaimDto(Claim claim)
		{
			this.Type = claim.Type;
			this.Value = claim.Value;
		}

		/// <summary>
		///   Gets or sets the type of the claim.
		/// </summary>
		[Required]
		[IsGuid(true, false)]
		public string Type { get; set; }

		/// <summary>
		///   Gets or sets the value of the claim.
		/// </summary>
		[Required]
		[IsGuid(true, false)]
		public string Value { get; set; }
	}
}