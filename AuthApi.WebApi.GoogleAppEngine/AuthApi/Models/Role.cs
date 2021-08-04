namespace AuthApi.Models
{
	using System;
	using AuthApi.Contracts;
	using Google.Cloud.Firestore;

	/// <summary>
	///   Describes an account role.
	/// </summary>
	[FirestoreData]
	public class Role : IRole
	{
		/// <summary>
		///   Creates a new instance of <see cref="Role" />.
		/// </summary>
		public Role()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="Role" />.
		/// </summary>
		/// <param name="id">The id of the role.</param>
		/// <param name="name">The name of the role.</param>
		public Role(string id, string name)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
			}

			this.Id = id;
			this.Name = name;
		}

		/// <summary>
		///   Creates a new instance of <see cref="Role" />.
		/// </summary>
		/// <param name="role">Data is initialized from the given role.</param>
		public Role(RoleDto role)
			: this(role.Id, role.Name)
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="Role" />.
		/// </summary>
		/// <param name="createRoleDto">Data is initialized from the given role.</param>
		public Role(CreateRoleDto createRoleDto)
			: this(Guid.NewGuid().ToString(), createRoleDto.Name)
		{
		}

		/// <summary>
		///   Gets or sets the id of the role.
		/// </summary>
		[FirestoreProperty("roleId")]
		public string Id { get; set; }

		/// <summary>
		///   Gets or sets the name of the role.
		/// </summary>
		[FirestoreProperty("name")]
		public string Name { get; set; }
	}
}