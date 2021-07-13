namespace StockApi
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///   Entry point of the application.
	/// </summary>
	public class Program
	{
		/// <summary>
		///   Entry point of the asp.net core application.
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		/// <summary>
		///   Create the host builder.
		/// </summary>
		/// <param name="args">Arguments from main entry point.</param>
		/// <returns>An instance implementing <see cref="IHostBuilder" />.</returns>
		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
		}
	}
}