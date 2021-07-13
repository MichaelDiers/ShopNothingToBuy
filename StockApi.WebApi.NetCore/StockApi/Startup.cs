namespace StockApi
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using StockApi.Contracts;
	using StockApi.Extensions;
	using StockApi.Services;

	/// <summary>
	///   ASP.NET Core apps use a Startup class, which is named Startup by convention.
	/// </summary>
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.

		/// <summary>
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpContextAccessor();
			services.HandleInvalidModelState();
			services.AddSingleton<IDatabaseService, DatabaseService>();
			services.AddSingleton<IStockService, StockService>();
		}

		/// <summary>
		///   The Configure method is used to specify how the app responds to HTTP requests. The request pipeline is configured by
		///   adding middleware components to an IApplicationBuilder instance. IApplicationBuilder is available to the Configure
		///   method, but it isn't registered in the service container. Hosting creates an IApplicationBuilder and passes it
		///   directly to Configure.
		/// </summary>
		/// <param name="app">An instance to configure the application's request pipeline.</param>
		/// <param name="env">Provides information about the web hosting environment an application is running in.</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}