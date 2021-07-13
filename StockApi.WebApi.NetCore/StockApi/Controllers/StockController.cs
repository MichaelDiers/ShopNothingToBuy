﻿namespace StockApi.Controllers
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   A controller that handles operations on <see cref="StockItemDto" /> instances.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class StockController : ControllerBase
	{
		/// <summary>
		///   Access the current http context.
		/// </summary>
		private readonly IHttpContextAccessor httpContextAccessor;

		/// <summary>
		///   Format string for the location of created <see cref="StockItemDto" /> instances.
		/// </summary>
		private readonly string stockCreateLocationFormat;

		/// <summary>
		///   Services for processing <see cref="StockItemDto" /> instances.
		/// </summary>
		private readonly IStockService stockService;

		/// <summary>
		///   Creates a new instance of <see cref="StockController" />.
		/// </summary>
		/// <param name="stockService">Service for processing <see cref="StockItemDto" /> instances.</param>
		/// <param name="httpContextAccessor">Accessor for the http context.</param>
		/// <param name="configuration">Access the configuration of the application.</param>
		/// <exception cref="ArgumentNullException">
		///   If <paramref name="stockService" />, <paramref name="httpContextAccessor" /> or
		///   <paramref name="configuration" /> is null.
		/// </exception>
		public StockController(IStockService stockService,
			IHttpContextAccessor httpContextAccessor,
			IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			this.stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
			this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
			this.stockCreateLocationFormat = configuration["StockCreateLocationFormat"];
		}

		/// <summary>
		///   Create a new <see cref="StockItemDto" /> in storage.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItemDto" /> to be created.</param>
		/// <returns>Status 201 if <see cref="StockItemDto" /> is created and 400 otherwise.</returns>
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] StockItemDto stockItem)
		{
			var isCreated = await this.stockService.Create(new StockItem(stockItem));
			if (isCreated)
			{
				return new CreatedResult(
					new Uri(string.Format(this.stockCreateLocationFormat, this.httpContextAccessor.HttpContext.Request.Host.Value,
						stockItem.Id)),
					stockItem);
			}

			return new BadRequestResult();
		}
	}
}