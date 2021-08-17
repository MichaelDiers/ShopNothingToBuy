namespace ShopNothingToBuy.Sdk.Extensions
{
	using System;
	using System.Net;
	using ShopNothingToBuy.Sdk.Contracts;

	/// <summary>
	///   Extensions for <see cref="DatabaseResult" />.
	/// </summary>
	public static class DatabaseResultExtensions
	{
		/// <summary>
		///   Maps a given <see cref="DatabaseResult" /> to <see cref="HttpStatusCode" />.
		/// </summary>
		/// <param name="databaseResult">The <see cref="DatabaseResult" /> to map.</param>
		/// <returns>The mapped <see cref="HttpStatusCode" />.</returns>
		public static HttpStatusCode ToHttpStatusCode(this DatabaseResult databaseResult)
		{
			switch (databaseResult)
			{
				case DatabaseResult.AlreadyExists:
					return HttpStatusCode.Conflict;
				case DatabaseResult.Error:
					return HttpStatusCode.InternalServerError;
				case DatabaseResult.Created:
					return HttpStatusCode.Created;
				case DatabaseResult.Deleted:
					return HttpStatusCode.NoContent;
				case DatabaseResult.NotFound:
					return HttpStatusCode.NotFound;
				case DatabaseResult.Updated:
					return HttpStatusCode.OK;
				default:
					throw new ArgumentOutOfRangeException(nameof(databaseResult), databaseResult, null);
			}
		}
	}
}