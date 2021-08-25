﻿namespace ShopNothingToBuy.Sdk.GoogleCloud
{
	/// <summary>
	///   Describes an empty request body.
	/// </summary>
	public class EmptyRequest : IIsValid
	{
		/// <summary>
		///   Checks if the object is valid.
		/// </summary>
		/// <returns>True if the object is valid and false otherwise.</returns>
		public bool IsValid()
		{
			return true;
		}
	}
}