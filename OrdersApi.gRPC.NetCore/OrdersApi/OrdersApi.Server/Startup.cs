namespace OrdersApi.Server
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using OrdersApi.Server.Contracts;
	using OrdersApi.Server.Interceptors;
	using OrdersApi.Server.Services;

	/// <summary>
	///   The <see cref="Startup" /> of the application.
	/// </summary>
	public class Startup
	{
		/// <summary>
		///   Configure the application pipeline.
		/// </summary>
		/// <param name="app">An <see cref="IApplicationBuilder" />.</param>
		/// <param name="env">The <see cref="IWebHostEnvironment" />.</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRouting();

			app.UseEndpoints(endpoints => { endpoints.MapGrpcService<OrderApi>(); });
		}

		/// <summary>
		///   Configure services and middleware.
		/// </summary>
		/// <param name="services">An <see cref="IServiceCollection" />.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IDatabaseService, DatabaseService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddLogging();
			services.AddGrpc(
				options =>
				{
					{
						options.Interceptors.Add<ApiKeyInterceptor>();
						options.Interceptors.Add<CreateOrderRequestInterceptor>();
						options.Interceptors.Add<CustomerIdRequestInterceptor>();
						options.Interceptors.Add<OrderIdRequestInterceptor>();
						options.Interceptors.Add<UpdateStatusRequestInterceptor>();
						options.Interceptors.Add<ErrorInterceptor>();
					}
				});
		}
	}
}