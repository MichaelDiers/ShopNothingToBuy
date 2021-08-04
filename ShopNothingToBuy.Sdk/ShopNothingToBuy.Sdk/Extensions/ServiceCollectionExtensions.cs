namespace ShopNothingToBuy.Sdk.Extensions
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///   Extensions for <see cref="IServiceCollection" />.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///   Suppress default behaviour of enriching empty bodies by the framework.
		/// </summary>
		/// <param name="services">An <see cref="IServiceCollection" /> for that additional options are set.</param>
		public static void AddSuppressMapClientErrors(this IServiceCollection services)
		{
			services.AddControllers().ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; });
		}

		/// <summary>
		///   Global handling for a invalid model state.
		/// </summary>
		/// <param name="services">An <see cref="IServiceCollection" /> to that the handling of errors is added.</param>
		public static void HandleInvalidModelState(this IServiceCollection services)
		{
			services.AddControllers()
				.ConfigureApiBehaviorOptions(
					options => { options.InvalidModelStateResponseFactory = context => new BadRequestResult(); });
		}
	}
}