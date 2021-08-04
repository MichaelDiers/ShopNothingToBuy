namespace ShopNothingToBuy.Sdk.Extensions
{
	using System.Net;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Builder;

	/// <summary>
	///   Extensions for <see cref="IApplicationBuilder" />.
	/// </summary>
	public static class ApplicationBuilderExtensions
	{
		/// <summary>
		///   In case of response <see cref="HttpStatusCode.UnsupportedMediaType" /> changes to
		///   <see cref="HttpStatusCode.BadRequest" />.
		///   Occurs if non-empty body is expected but the request is sent with an empty body.
		/// </summary>
		/// <param name="app">An <see cref="IApplicationBuilder" /> implementing instance.</param>
		public static void ChangeHttpStatus415To400(this IApplicationBuilder app)
		{
			app.UseStatusCodePages(
				context =>
				{
					var statusCode = context.HttpContext.Response.StatusCode;
					if (statusCode == (int) HttpStatusCode.UnsupportedMediaType)
					{
						context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
					}

					return Task.CompletedTask;
				});
		}
	}
}