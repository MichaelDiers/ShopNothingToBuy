namespace ShopNothingToBuy.Sdk.Attributes
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class IsGuidAttribute : ValidationAttribute
	{
		private readonly bool allowEmptyGuid;
		private readonly bool isRequired;

		public IsGuidAttribute(bool isRequired, bool allowEmptyGuid)
		{
			this.isRequired = isRequired;
			this.allowEmptyGuid = allowEmptyGuid;
		}

		public IsGuidAttribute(bool isRequired)
			: this(isRequired, false)
		{
		}

		public IsGuidAttribute()
			: this(true, false)
		{
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null)
			{
				return this.isRequired ? new ValidationResult("guid is required") : ValidationResult.Success;
			}

			var guidAsString = value.ToString();
			if (string.IsNullOrWhiteSpace(guidAsString))
			{
				return this.isRequired ? new ValidationResult("guid is required") : ValidationResult.Success;
			}

			if (Guid.TryParse(guidAsString, out var guid) && (guid != Guid.Empty || this.allowEmptyGuid))
			{
				return ValidationResult.Success;
			}

			return new ValidationResult("invalid guid");
		}
	}
}