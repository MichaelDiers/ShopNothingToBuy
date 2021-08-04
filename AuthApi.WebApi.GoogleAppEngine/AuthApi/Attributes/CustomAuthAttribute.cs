namespace AuthApi.Attributes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AuthApi.Contracts;
	using ShopNothingToBuy.Sdk.Attributes;

	/// <summary>
	///   Handles authorization for the application. Maps roles as <see cref="AuthRole" /> to its id as <see cref="Guid" />.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class CustomAuthAttribute : AuthAttribute
	{
		/// <summary>
		///   The id of the <see cref="AuthApi" /> application.
		/// </summary>
		private const string Application = "d46ebb2a-32cf-43d0-a0c2-570f5935e47c";

		/// <summary>
		///   The id for the admin role.
		/// </summary>
		private const string RoleAdmin = "5109f724-02a3-400f-84b8-22b12976cdec";

		/// <summary>
		///   The id for the service reader role.
		/// </summary>
		private const string RoleServiceReader = "e25bb437-b32d-4be0-a054-9c84fc2889b9";

		/// <summary>
		///   The id for the service writer role.
		/// </summary>
		private const string RoleServiceWriter = "a95cdab1-9bef-4181-9827-38b2305ad7d8";

		/// <summary>
		///   The id for the user role.
		/// </summary>
		private const string RoleUser = "987221a6-05b1-46ba-911d-2576b19df2a8";

		/// <summary>
		///   Creates a new instance of the <see cref="CustomAuthAttribute" />.
		/// </summary>
		/// <param name="acceptRefreshToken">Specifies if the method accepts refresh tokens (true) or regular tokens (false).</param>
		/// <param name="allowedRoles">The list of allowed roles for the called method.</param>
		public CustomAuthAttribute(bool acceptRefreshToken, params AuthRole[] allowedRoles)
			: base(acceptRefreshToken, Application, Map(allowedRoles))
		{
		}

		/// <summary>
		///   Creates a new instance of the <see cref="CustomAuthAttribute" />.
		/// </summary>
		/// <param name="allowedRoles">The list of allowed roles for the called method.</param>
		/// <remarks>Refresh tokens will be rejected.</remarks>
		public CustomAuthAttribute(params AuthRole[] allowedRoles)
			: this(false, allowedRoles)
		{
		}

		/// <summary>
		///   Maps roles as <see cref="AuthRole" /> to its id as <see cref="Guid" /> as <see cref="string" />.
		/// </summary>
		/// <param name="roles">The <see cref="IEnumerable{T}" /> of <see cref="AuthRole" /> to be mapped.</param>
		/// <returns>An <see cref="Array" /> of <see cref="string" /> containing the ids of the given roles.</returns>
		private static string[] Map(IEnumerable<AuthRole> roles)
		{
			var mappedRoles = new List<string>();
			foreach (var role in roles)
			{
				mappedRoles.AddRange(Map(role));
			}

			return mappedRoles.ToArray();
		}

		/// <summary>
		///   Map a <see cref="AuthRole" /> to its id.
		/// </summary>
		/// <param name="authRole">The role to be mapped.</param>
		/// <returns>A <see cref="string" /> that represents the id of the role.</returns>
		/// <exception cref="ArgumentException">Is thrown if <paramref name="authRole" /> equals <see cref="AuthRole.None" />.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Is thrown if <paramref name="authRole" /> is not handled.</exception>
		private static IEnumerable<string> Map(AuthRole authRole)
		{
			return authRole switch
			{
				AuthRole.None => throw new ArgumentException("Undefined authRole (None)"),
				AuthRole.User => new[]
				{
					RoleUser
				},
				AuthRole.Reader => new[]
				{
					RoleServiceReader
				},
				AuthRole.Writer => new[]
				{
					RoleServiceWriter
				},
				AuthRole.Admin => new[]
				{
					RoleAdmin
				},
				AuthRole.All => MapAll(),
				_ => throw new ArgumentOutOfRangeException(nameof(authRole), authRole, null)
			};
		}

		/// <summary>
		///   Maps <see cref="AuthRole.All" /> to the matching role ids.
		/// </summary>
		/// <returns>An array of <see cref="string" />.</returns>
		private static string[] MapAll()
		{
			return Enum.GetNames(typeof(AuthRole)).Select(Enum.Parse<AuthRole>)
				.Where(value => value != AuthRole.None && value != AuthRole.All).Select(value => Map(value).First()).ToArray();
		}
	}
}