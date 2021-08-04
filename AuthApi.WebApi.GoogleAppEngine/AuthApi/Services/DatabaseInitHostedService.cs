namespace AuthApi.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using AuthApi.Contracts;
	using AuthApi.Models;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///   Initialize the database.
	/// </summary>
	public class DatabaseInitHostedService : IHostedService
	{
		/// <summary>
		///   Service for accessing <see cref="IAccount" /> instances.
		/// </summary>
		private readonly IAccountService accountService;

		/// <summary>
		///   Service for accessing <see cref="IApplication" /> instances.
		/// </summary>
		private readonly IApplicationService applicationService;

		/// <summary>
		///   Access the configuration of the application.
		/// </summary>
		private readonly IConfiguration configuration;

		/// <summary>
		///   Creates a new instance of <see cref="DatabaseInitHostedService" />.
		/// </summary>
		/// <param name="configuration">The configuration of the application.</param>
		/// <param name="applicationService">Service for accessing <see cref="IApplication" /> instances.</param>
		/// <param name="accountService">Service for accessing <see cref="IAccount" /> instances.</param>
		public DatabaseInitHostedService(
			IConfiguration configuration,
			IApplicationService applicationService,
			IAccountService accountService)
		{
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
			this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
		}

		/// <summary>
		///   Creates applications and accounts at application startup.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="CancellationToken" />.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var section = this.configuration.GetSection("DatabaseInit");

			if (section is null || !section.GetValue<bool>("Execute"))
			{
				return;
			}

			var applications = new List<ApplicationDto>();
			this.configuration.Bind("DatabaseInit:Applications", applications);
			if (applications.Any())
			{
				foreach (var application in applications)
				{
					var _ = await this.applicationService.Create(new Application(application));
				}
			}

			var accounts = new List<AccountDto>();
			this.configuration.Bind("DatabaseInit:Accounts", accounts);
			if (accounts.Any())
			{
				foreach (var account in accounts)
				{
					var _ = await this.accountService.Create(new Account(account));
				}
			}
		}

		/// <summary>
		///   The process started from <see cref="StartAsync" /> is not stopped.
		/// </summary>
		/// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}