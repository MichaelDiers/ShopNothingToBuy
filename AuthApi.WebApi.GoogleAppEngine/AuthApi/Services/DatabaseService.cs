namespace AuthApi.Services
{
	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Threading.Tasks;
	using AuthApi.Contracts;
	using AuthApi.Models;
	using Google.Cloud.Firestore;
	using Grpc.Core;
	using Microsoft.Extensions.Configuration;

	/// <summary>
	///   Implements all database operations of the application.
	/// </summary>
	public class DatabaseService : IAccountDatabaseService, IAuthDatabaseService, IApplicationDatabaseService
	{
		/// <summary>
		///   The used firestore database.
		/// </summary>
		private readonly FirestoreDb database;

		/// <summary>
		///   The database access configuration.
		/// </summary>
		private readonly DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration();

		/// <summary>
		///   Creates a new instance of <see cref="DatabaseService" />.
		/// </summary>
		/// <param name="configuration">The configuration of the application.</param>
		public DatabaseService(IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			configuration.Bind("Database", this.databaseConfiguration);
			if (!this.databaseConfiguration.IsValid())
			{
				throw new ArgumentException("Invalid database configuration");
			}

			this.database = FirestoreDb.Create(this.databaseConfiguration.GoogleProject);
		}

		/// <summary>
		///   Create a new account.
		/// </summary>
		/// <param name="account">The account to be created.</param>
		/// <returns>True if the account is created and otherwise false.</returns>
		public async Task<bool> Create(IAccount account)
		{
			try
			{
				var docRef = this.Collection(this.databaseConfiguration.CollectionNameAccounts).Document(account.Name);
				var _ = await docRef.CreateAsync(account);

				return true;
			}
			catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
			{
				return false;
			}
		}

		/// <summary>
		///   Delete an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The account to be deleted.</param>
		/// <returns>True if the account is deleted and false otherwise.</returns>
		public async Task<bool> DeleteAccount(string accountName)
		{
			return await this.Delete(this.databaseConfiguration.CollectionNameAccounts, accountName);
		}

		/// <summary>
		///   Delete all accounts of the application.
		/// </summary>
		/// <returns></returns>
		public async Task<bool> DeleteAccounts()
		{
			return await DeleteAll(
				async () => await this.ReadAccounts(),
				entry => entry.Name,
				async id => await this.DeleteAccount(id));
		}

		/// <summary>
		///   Read an account by its <see cref="IAccount.Name" />.
		/// </summary>
		/// <param name="accountName">The name of the account.</param>
		/// <returns>An instance of <see cref="IAccount" /> or null if not found.</returns>
		public async Task<IAccount> ReadAccount(string accountName)
		{
			var docRef = this.Collection(this.databaseConfiguration.CollectionNameAccounts).Document(accountName);
			var snapshot = await docRef.GetSnapshotAsync();
			return snapshot.Exists ? snapshot.ConvertTo<Account>() : null;
		}

		/// <summary>
		///   Read all accounts from the database.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IAccount" />.</returns>
		public async Task<IEnumerable<IAccount>> ReadAccounts()
		{
			return await this.Read<Account>(this.databaseConfiguration.CollectionNameAccounts);
		}

		/// <summary>
		///   Read applications by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="applicationIds">The ids of the requested applications.</param>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IApplication" />.</returns>
		public async Task<IEnumerable<IApplication>> ReadApplications(IReadOnlyCollection<string> applicationIds)
		{
			var applications = new List<Application>();
			var snapshot = await this.Collection(this.databaseConfiguration.CollectionNameApplications).WhereIn(
				new FieldPath(Application.DatabaseApplicationId),
				applicationIds).GetSnapshotAsync();
			if (snapshot.Count > 0)
			{
				applications.AddRange(
					snapshot.Documents.Where(documentSnapshot => documentSnapshot.Exists)
						.Select(documentSnapshot => documentSnapshot.ConvertTo<Application>()));
			}

			return applications;
		}

		/// <summary>
		///   Replace the database document <see cref="IAccount.Name" /> by the contents of <paramref name="account" />.
		/// </summary>
		/// <param name="account">The new values for the account. The <see cref="IAccount.Name" /> is used as unique reference.</param>
		/// <returns>True if the account is updated and false otherwise.</returns>
		public async Task<bool> Update(IAccount account)
		{
			if (string.IsNullOrWhiteSpace(account.Password))
			{
				throw new ArgumentNullException(nameof(IAccount.Password));
			}

			var _ = await this.Collection(this.databaseConfiguration.CollectionNameAccounts).Document(account.Name)
				.SetAsync(account, SetOptions.Overwrite);
			return true;
		}

		/// <summary>
		///   Create a new application.
		/// </summary>
		/// <param name="application">The application data.</param>
		/// <returns>True if the application is created and false otherwise.</returns>
		public async Task<bool> Create(IApplication application)
		{
			try
			{
				var docRef = this.Collection(this.databaseConfiguration.CollectionNameApplications)
					.Document(application.Id);
				var _ = await docRef.CreateAsync(application);

				return true;
			}
			catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
			{
				return false;
			}
		}

		/// <summary>
		///   Delete an application by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>True if the application is deleted and false otherwise.</returns>
		public async Task<bool> DeleteApplication(string applicationId)
		{
			return await this.Delete(this.databaseConfiguration.CollectionNameApplications, applicationId);
		}

		/// <summary>
		///   Delete all applications.
		/// </summary>
		/// <returns>True if the operations succeeds and false otherwise.</returns>
		public async Task<bool> DeleteApplications()
		{
			return await DeleteAll(
				async () => await this.ReadApplications(),
				entry => entry.Id,
				async id => await this.DeleteApplication(id));
		}

		/// <summary>
		///   Read an application by its <see cref="IApplication.Id" />.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>An instance of <see cref="IApplication" /> or null if no application with given id exists.</returns>
		public async Task<IApplication> ReadApplication(string applicationId)
		{
			var snapshot = await this.Collection(this.databaseConfiguration.CollectionNameApplications)
				.Document(applicationId)
				.GetSnapshotAsync();
			return snapshot.Exists ? snapshot.ConvertTo<Application>() : null;
		}

		/// <summary>
		///   Read all applications.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}" /> of <see cref="IApplication" />.</returns>
		public async Task<IEnumerable<IApplication>> ReadApplications()
		{
			return await this.Read<Application>(this.databaseConfiguration.CollectionNameApplications);
		}

		/// <summary>
		///   Add a refresh token to the database.
		/// </summary>
		/// <param name="refreshToken">The token to be added.</param>
		/// <returns>True if the token is added and false otherwise.</returns>
		public async Task AddRefreshToken(string refreshToken)
		{
			var docRef = this.Collection(this.databaseConfiguration.CollectionNameRefreshTokens).Document();
			var token = new RefreshToken
			{
				Token = refreshToken,
				RefreshCount = 0
			};

			var _ = await docRef.CreateAsync(token);
		}

		/// <summary>
		///   Delete all refresh tokens with a refresh count of more than <see paramref="maxRefreshCount" /> or that are expired
		///   according to <paramref name="validToValidation" />.
		/// </summary>
		/// <param name="maxRefreshCount">The maximum allowed refresh count for a refresh token.</param>
		/// <param name="validToValidation">Checks if a refresh token is already expired.</param>
		/// <returns>A <see cref="Task" />.</returns>
		public async Task RemoveInvalidRefreshTokens(int maxRefreshCount, Func<DateTime, bool> validToValidation)
		{
			var snapshot = await this.Collection(this.databaseConfiguration.CollectionNameRefreshTokens).GetSnapshotAsync();
			if (snapshot.Count > 0)
			{
				var batch = this.database.StartBatch();
				var batchSize = 0;
				foreach (var documentSnapshot in snapshot.Documents.Where(ds => ds.Exists))
				{
					var tokenData = documentSnapshot.ConvertTo<RefreshToken>();
					if (tokenData.RefreshCount > maxRefreshCount)
					{
						batch.Delete(documentSnapshot.Reference);
						++batchSize;
					}
					else
					{
						var jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenData.Token);
						if (!validToValidation(jwt.ValidTo))
						{
							batch.Delete(documentSnapshot.Reference);
							batchSize++;
						}
					}

					if (batchSize > 100)
					{
						await batch.CommitAsync();
						batch = this.database.StartBatch();
						batchSize = 0;
					}
				}

				if (batchSize > 0)
				{
					await batch.CommitAsync();
				}
			}
		}

		/// <summary>
		///   Replace an old refresh token by a new one.
		/// </summary>
		/// <param name="oldRefreshToken">This refresh token gets invalid and is removed from storage.</param>
		/// <param name="newRefreshToken">The new an valid token that is added to storage.</param>
		/// <returns>The current refresh count of the token or -1 if the operation failed.</returns>
		public async Task<int> ReplaceRefreshToken(string oldRefreshToken, string newRefreshToken)
		{
			var snapshot = await this.Collection(this.databaseConfiguration.CollectionNameRefreshTokens)
				.WhereEqualTo(RefreshToken.DatabaseTokenName, oldRefreshToken).GetSnapshotAsync();
			if (snapshot.Count > 0)
			{
				var documentSnapshot = snapshot.Documents.FirstOrDefault(ds => ds.Exists);
				var refreshTokenDatabase = documentSnapshot?.ConvertTo<RefreshToken>();
				if (refreshTokenDatabase != null && !string.IsNullOrWhiteSpace(refreshTokenDatabase.Token))
				{
					var newRefreshTokenDatabase = new RefreshToken
					{
						Token = newRefreshToken,
						RefreshCount = refreshTokenDatabase.RefreshCount + 1
					};

					var _ = await this.Collection(this.databaseConfiguration.CollectionNameRefreshTokens)
						.Document(documentSnapshot.Id)
						.SetAsync(
							newRefreshTokenDatabase,
							SetOptions.Overwrite);
					return newRefreshTokenDatabase.RefreshCount;
				}
			}

			return -1;
		}

		/// <summary>
		///   Creates a local <see cref="CollectionReference" /> for the given <paramref name="path" />.
		/// </summary>
		/// <param name="path">The name of the collection.</param>
		/// <returns>A <see cref="CollectionReference" />.</returns>
		private CollectionReference Collection(string path)
		{
			return this.database.Collection(path);
		}

		/// <summary>
		///   Delete an entry from the database.
		/// </summary>
		/// <param name="collectionName">The name of the collection from that the entry is deleted.</param>
		/// <param name="documentId">The id of the document.</param>
		/// <returns>True if the entry is deleted and false otherwise.</returns>
		private async Task<bool> Delete(string collectionName, string documentId)
		{
			try
			{
				var docRef = this.Collection(collectionName).Document(documentId);
				var _ = await docRef.DeleteAsync(Precondition.MustExist);

				return true;
			}
			catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
			{
				return false;
			}
		}

		/// <summary>
		///   Delete all entries from a collection.
		/// </summary>
		/// <typeparam name="TEntry">The type of the collection entries.</typeparam>
		/// <typeparam name="TId">The type of the id used for entries.</typeparam>
		/// <param name="listEntries">A function that returns all entries of a collection.</param>
		/// <param name="readEntryId">A function that reads the id from an entry.</param>
		/// <param name="deleteEntry">A function that deletes a single entry.</param>
		/// <returns>True if all entries are deleted and false otherwise.</returns>
		private static async Task<bool> DeleteAll<TEntry, TId>(
			Func<Task<IEnumerable<TEntry>>> listEntries,
			Func<TEntry, TId> readEntryId,
			Func<TId, Task<bool>> deleteEntry)
		{
			var entries = await listEntries();
			foreach (var entry in entries)
			{
				if (!await deleteEntry(readEntryId(entry)))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		///   Reads all entries from a collection.
		/// </summary>
		/// <typeparam name="T">The type of the collection entries.</typeparam>
		/// <param name="collectionName">The name of the collection.</param>
		/// <returns>An <see cref="IEnumerable{T}" /> of the collection entries.</returns>
		private async Task<IEnumerable<T>> Read<T>(string collectionName)
		{
			var results = new List<T>();
			var snapshot = await this.Collection(collectionName).GetSnapshotAsync();
			if (snapshot.Count > 0)
			{
				results.AddRange(
					snapshot.Documents.Where(documentSnapshot => documentSnapshot.Exists)
						.Select(documentSnapshot => documentSnapshot.ConvertTo<T>()));
			}

			return results;
		}
	}
}