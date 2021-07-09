using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProductsApi.Contracts;
using ProductsApi.Services;

using System.Threading.Tasks;

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
			
			builder.Services.AddSingleton((s) =>
			{
				CosmosClientBuilder cosmosClientBuilder = new CosmosClientBuilder();
				return cosmosClientBuilder.WithConnectionModeDirect()
					.Build();
			});

			// builder.Services.AddLogging();
		}		
	}
}
