namespace Service.Sdk.Tests.Models
{
	internal class CreateEntry
	{
		public CreateEntry(int value)
		{
			this.Value = value;
		}

		public int Value { get; }
	}
}