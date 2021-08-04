namespace AuthApi.Tests.Converter
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Security.Claims;
	using AuthApi.Converter;
	using Google.Cloud.Firestore;
	using Xunit;

	public class ClaimCollectionFirestoreConverterTests
	{
		[Fact]
		public void Constructor()
		{
			Assert.IsAssignableFrom<IFirestoreConverter<IReadOnlyCollection<Claim>>>(new ClaimCollectionFirestoreConverter());
		}

		[Fact]
		public void FromFirestroreReturnsEmptyForEmpty()
		{
			var converter = new ClaimCollectionFirestoreConverter();
			Assert.Empty(converter.FromFirestore(new List<object>()));
		}

		[Fact]
		public void FromFirestroreReturnsNonEmptyForNonEmpty()
		{
			var data = new List<IDictionary<string, object>>
			{
				new Dictionary<string, object>
				{
					{"type", "my type"},
					{"value", "my value"}
				},
				new Dictionary<string, object>
				{
					{"type", "my type 2"},
					{"value", "my value 2"}
				}
			};
			var converter = new ClaimCollectionFirestoreConverter();
			var result = converter.FromFirestore(data);
			foreach (var dict in data)
			{
				Assert.True(result.Any(claim => claim.Type == dict["type"] && claim.Value == dict["value"]));
			}
		}

		[Fact]
		public void FromFirestroreReturnsNullForInvalidInput()
		{
			var converter = new ClaimCollectionFirestoreConverter();
			Assert.Null(converter.FromFirestore("wrorng input"));
		}

		[Fact]
		public void FromFirestroreReturnsNullForNull()
		{
			var converter = new ClaimCollectionFirestoreConverter();
			Assert.Null(converter.FromFirestore(null));
		}

		[Fact]
		public void ToFirestoreEmptyForEmpty()
		{
			var converter = new ClaimCollectionFirestoreConverter();
			var result = converter.ToFirestore(new ReadOnlyCollection<Claim>(new List<Claim>())) as IEnumerable;
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void ToFirestoreListOfDynamicForNonEmptyList()
		{
			var converter = new ClaimCollectionFirestoreConverter();
			var data = new ReadOnlyCollection<Claim>(
				new List<Claim>
				{
					new Claim("mytype", "my value"),
					new Claim("your type", "your value")
				});

			var result = converter.ToFirestore(new ReadOnlyCollection<Claim>(data)) as IEnumerable;
			Assert.NotNull(result);

			foreach (var obj in result)
			{
				var type = obj.GetType().GetProperty("type").GetValue(obj);
				var value = obj.GetType().GetProperty("value").GetValue(obj);

				Assert.True(data.Any(expectedClaim => expectedClaim.Type == type && expectedClaim.Value == value));
			}
		}

		[Fact]
		public void ToFirestoreThrowsArgumentNullExceptionForNull()
		{
			var converter = new ClaimCollectionFirestoreConverter();
			Assert.Throws<ArgumentNullException>(() => converter.ToFirestore(null));
		}
	}
}