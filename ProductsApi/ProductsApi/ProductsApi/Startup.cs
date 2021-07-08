using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using ProductsApi.Contracts;
using ProductsApi.Services;

[assembly: FunctionsStartup(typeof(ProductsApi.Startup))]

namespace ProductsApi
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      builder.Services.AddSingleton<IProductsService, ProductsService>();
    }
  }
}
