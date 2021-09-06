﻿namespace Application.Contracts
{
	using Service.Sdk.Contracts;

	/// <summary>
	///   Describes operations on <see cref="ApplicationEntry" /> instances.
	/// </summary>
	public interface
		IApplicationService : IServiceBase<ApplicationEntry, string, CreateApplicationEntry, UpdateApplicationEntry>
	{
	}
}