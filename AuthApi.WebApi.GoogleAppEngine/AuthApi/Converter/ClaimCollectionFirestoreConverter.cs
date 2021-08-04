namespace AuthApi.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using Google.Cloud.Firestore;

	/// <summary>
	///   Converter for firestore data.
	/// </summary>
	public class ClaimCollectionFirestoreConverter : IFirestoreConverter<IReadOnlyCollection<Claim>>
	{
		/// <summary>
		///   Converts an <see cref="object" /> to a <see cref="IReadOnlyCollection{T}" /> of <see cref="Guid" />.
		/// </summary>
		/// <param name="value">Data from firestore that parseable to <see cref="IEnumerable{T}" /> of <see cref="string" />.</param>
		/// <returns>An <see cref="IReadOnlyCollection{T}" /> of <see cref="Guid" />.</returns>
		public IReadOnlyCollection<Claim> FromFirestore(object value)
		{
			if (value is IEnumerable<object> claims)
			{
				return claims
					.Select(
						claim => claim as IDictionary<string, object>
						         ?? throw new ArgumentNullException(nameof(value), "Invalid claim data from database")).Select(
						claim => new Claim(claim["type"] as string, claim["value"] as string)).ToArray();
			}

			return null;
		}

		/// <summary>
		///   Convert an <see cref="IReadOnlyCollection{T}" /> of <see cref="Guid" /> to an array of <see cref="string" />.
		/// </summary>
		/// <param name="value">Data to convert.</param>
		/// <returns>An array of <see cref="string" /> as <see cref="object" />.</returns>
		public object ToFirestore(IReadOnlyCollection<Claim> value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			return value.Select(
				claim => new
				{
					type = claim.Type,
					value = claim.Value
				}).ToArray();
		}
	}
}