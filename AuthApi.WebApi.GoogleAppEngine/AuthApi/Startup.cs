namespace AuthApi
{
	using AuthApi.Contracts;
	using AuthApi.Services;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.DependencyInjection;
	using ShopNothingToBuy.Sdk.Contracts;
	using ShopNothingToBuy.Sdk.Extensions;
	using ShopNothingToBuy.Sdk.Services;

	/// <summary>
	///   ASP.NET Core apps use a Startup class, which is named Startup by convention.
	/// </summary>
	public class Startup
	{
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
			app.ChangeHttpStatus415To400();
			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}

		/// <summary>
		///   This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services">An <see cref="IServiceCollection" />.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpContextAccessor();
			services.HandleInvalidModelState();
			services.AddSuppressMapClientErrors();
			services.AddControllers();

			services.AddSingleton<IApplicationService, ApplicationService>();
			services.AddSingleton<IAccountService, AccountService>();
			services.AddSingleton<IAuthService, AuthService>();
			services.AddSingleton<IJwtService, JwtService>();

			services.AddSingleton<IAccountDatabaseService, DatabaseService>();
			services.AddSingleton<IApplicationDatabaseService, DatabaseService>();
			services.AddSingleton<IAuthDatabaseService, DatabaseService>();

			services.AddHostedService<DatabaseInitHostedService>();
			services.AddHostedService<RemoveInvalidRefreshTokensHostedService>();
		}
	}
}