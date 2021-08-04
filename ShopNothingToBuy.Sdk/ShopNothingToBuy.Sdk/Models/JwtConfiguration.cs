namespace ShopNothingToBuy.Sdk.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class JwtConfiguration
	{
		public string Audience { get; set; }

		public int ExpiresRefresh { get; set; }
		public int ExpiresToken { get; set; }
		public string Issuer { get; set; }
		public IReadOnlyCollection<JwtConfigurationKey> Keys { get; set; }

		public int KeyVersion { get; set; }
		public int MaxRefreshCount { get; set; }

		public bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(this.Audience)
			       && this.ExpiresToken > 0
			       && this.ExpiresRefresh > this.ExpiresToken
			       && !string.IsNullOrWhiteSpace(this.Issuer)
			       && this.KeyVersion > 0
			       && this.Keys != null
			       && this.Keys.Any(key => key.Version == this.KeyVersion)
			       && this.MaxRefreshCount > 0;
		}

		public byte[] PrivateKeyAsByteArray()
		{
			return Convert.FromBase64String(this.Keys.First(key => key.Version == this.KeyVersion).PrivateKey);
		}

		public byte[] PublicKeyAsByteArray(int version)
		{
			var key = this.Keys.FirstOrDefault(keyEntry => keyEntry.Version == version)?.PublicKey;
			return string.IsNullOrWhiteSpace(key) ? null : Convert.FromBase64String(key);
		}
	}
}