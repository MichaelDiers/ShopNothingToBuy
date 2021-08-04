namespace AuthApi.Services
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using AuthApi.Contracts;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///   Cleanup operation for invalid refresh tokens in the database.
	/// </summary>
	public class RemoveInvalidRefreshTokensHostedService : IHostedService
	{
		/// <summary>
		///   Service for authentication.
		/// </summary>
		private readonly IAuthDatabaseService authDatabaseService;

		/// <summary>
		///   The configuration of the application.
		/// </summary>
		private readonly IConfiguration configuration;

		/// <summary>
		///   Creates a new instance of <see cref="RemoveInvalidRefreshTokensHostedService" />.
		/// </summary>
		/// <param name="authDatabaseService">Service for authentication.</param>
		/// <param name="configuration">The configuration of the application.</param>
		public RemoveInvalidRefreshTokensHostedService(
			IAuthDatabaseService authDatabaseService,
			IConfiguration configuration)
		{
			this.authDatabaseService = authDatabaseService ?? throw new ArgumentNullException(nameof(authDatabaseService));
			this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		/// <summary>
		///   Deletes all invalid refresh tokens.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="CancellationToken" />.</param>
		/// <returns>A <see cref="Task" />.</returns>
		/// <seealso cref="IAuthDatabaseService.RemoveInvalidRefreshTokens" />
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await this.authDatabaseService.RemoveInvalidRefreshTokens(
				this.configuration.GetValue<int>("Jwt:MaxRefreshCount"),
				dateTime => DateTime.UtcNow < dateTime.ToUniversalTime());
		}

		/// <summary>
		///   The operation started at <see cref="StartAsync" /> is not terminated.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="CancellationToken" />.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}