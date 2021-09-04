namespace User.Services.Models
{
	/// <summary>
	///   User data that is used in <see cref="UserEntry" />, <see cref="CreateUserEntry" /> and <see cref="UpdateUserEntry" />
	///   .
	/// </summary>
	public class BaseUser
	{
		/// <summary>
		///   Creates a new instance of <see cref="BaseUser" />.
		/// </summary>
		public BaseUser()
		{
		}

		/// <summary>
		///   Creates a new instance of <see cref="BaseUser" />.
		/// </summary>
		/// <param name="name"></param>
		public BaseUser(string name)
		{
			this.Name = name;
		}

		/// <summary>
		///   Gets or sets the name of the user.
		/// </summary>
		public string Name { get; set; }
	}
}