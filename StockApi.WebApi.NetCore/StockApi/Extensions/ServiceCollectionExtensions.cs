namespace StockApi.Extensions
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///   Extensions for <see cref="IServiceCollection" />.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///   Global handling for a invalid model state.
		/// </summary>
		/// <param name="services">An <see cref="IServiceCollection" /> to that the handling of errors is added.</param>
		public static void HandleInvalidModelState(this IServiceCollection services)
		{
			services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					// add string.Empty in order to send with an empty body
					options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(string.Empty);
				});
		}
	}
}