using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using ProductsApi.Contracts;
using ProductsApi.Services;

[assembly: FunctionsStartup(typeof(ProductsApi.Startup))]

namespace ProductsApi
{
	/// <summary>
	/// Startup definition for azure functions.
	/// </summary>
	public class Startup : FunctionsStartup
	{
		/// <summary>
		/// Performs the startup configuration action. The runtime will call this method at the right time during initialization.
		/// </summary>
		/// <param name="builder">The <see cref="IFunctionsHostBuilder"/> that can be used to
		/// configure the host.</param>
		public override void Configure(IFunctionsHostBuilder builder)
		{
			// depency injections
			builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
			builder.Services.AddSingleton<IProductsService, ProductsService>();
		}
	}
}
