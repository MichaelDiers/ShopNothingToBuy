namespace Authentication.Contracts
{
	public enum CreateResult
	{
		None = 0,
		Created = 1,
		AlreadyExists = 2,
		InternalError = 3
	}
}