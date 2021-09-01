namespace Authentication.Contracts
{
	public interface IEntry<out T>
	{
		T Id { get; }
	}
}