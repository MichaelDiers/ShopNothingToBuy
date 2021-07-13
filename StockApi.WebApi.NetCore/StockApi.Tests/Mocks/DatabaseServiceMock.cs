﻿namespace StockApi.Tests.Mocks
{
	using System;
	using System.Threading.Tasks;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   Mock for <see cref="IDatabaseService" />.
	/// </summary>
	public class DatabaseServiceMock : IDatabaseService
	{
		/// <summary>
		///   Specifies if the operation should succeed or fail.
		/// </summary>
		private readonly bool defaultReturnValue;

		/// <summary>
		///   Create a new instance of <see cref="DatabaseServiceMock" />.
		/// </summary>
		/// <param name="defaultReturnValue">Specifies if the operation should succeed or fail.</param>
		public DatabaseServiceMock(bool defaultReturnValue)
		{
			this.defaultReturnValue = defaultReturnValue;
		}

		/// <summary>
		///   Fakes to create a <see cref="StockItem" />.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItem" /> to be created.</param>
		/// <returns>The value specified with <see cref="defaultReturnValue" />.</returns>
		public Task<bool> Create(StockItem stockItem)
		{
			return Task.FromResult(this.defaultReturnValue);
		}

		/// <summary>
		///   Fakes read by id.
		/// </summary>
		/// <param name="id">The id of the <see cref="StockItem" />.</param>
		/// <returns>
		///   A <see cref="StockItem" /> with given <paramref name="id" /> if <see cref="defaultReturnValue" /> is true and
		///   null otherwise.
		/// </returns>
		public Task<StockItem> ReadById(Guid id)
		{
			return Task.FromResult(this.defaultReturnValue ? new StockItem {Id = id, InStock = 1000} : null);
		}
	}
}