namespace OrdersApi.Server
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///   Initialize and run the application.
	/// </summary>
	public class Program
	{
		/// <summary>
		///   Defines the <see cref="Startup" /> of the application.
		/// </summary>
		/// <param name="args">Args for the <see cref="IHostBuilder" />.</param>
		/// <returns>An instance of <see cref="IHostBuilder" />.</returns>
		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
		}

		/// <summary>
		///   Start the application.
		/// </summary>
		/// <param name="args">Args used for <see cref="CreateHostBuilder" />.</param>
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}
	}
}