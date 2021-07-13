namespace StockApi.Validation
{
	using System;
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///   Reject <see cref="Guid.Empty" /> values.
	/// </summary>
	public class NonEmptyGuidAttribute : ValidationAttribute
	{
		/// <summary>
		///   Indicates if the given <paramref name="value" /> equals <see cref="Guid.Empty" />.
		/// </summary>
		/// <param name="value">The guid to be checked.</param>
		/// <param name="validationContext">The current validation context.</param>
		/// <returns><see cref="ValidationResult.Success" /> if the <see cref="Guid" /> is not empty and false otherwise.</returns>
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is null)
			{
				return new ValidationResult("Guid is required.");
			}

			var guid = (Guid) value;
			return guid != Guid.Empty ? ValidationResult.Success : new ValidationResult("Empty guid not allowed.");
		}
	}
}