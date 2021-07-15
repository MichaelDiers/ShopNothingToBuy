namespace StockApi.Controllers
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using StockApi.Attributes;
	using StockApi.Contracts;
	using StockApi.Models;

	/// <summary>
	///   A controller that handles operations on <see cref="StockItemDto" /> instances.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiKey]
	public class StockController : ControllerBase
	{
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
		/// <param name="configuration">Access the configuration of the application.</param>
		/// <exception cref="ArgumentNullException">
		///   If <paramref name="stockService" /> or <paramref name="configuration" /> is null.
		/// </exception>
		public StockController(IStockService stockService, IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			this.stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
			this.stockCreateLocationFormat = configuration["StockCreateLocationFormat"];
		}

		/// <summary>
		///   Delete all entries from the storage.
		/// </summary>
		/// <returns>True if operation succeeded and false otherwise.</returns>
		[HttpDelete]
		[Route("all")]
		public async Task<IActionResult> Clear()
		{
			var deleted = await this.stockService.Clear();
			return deleted ? new OkResult() : new StatusCodeResult(500);
		}

		/// <summary>
		///   Create a new <see cref="StockItemDto" /> in storage.
		/// </summary>
		/// <param name="stockItem">The <see cref="StockItemDto" /> to be created.</param>
		/// <returns>Status 201 if <see cref="StockItemDto" /> is created and 400 otherwise.</returns>
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] StockItemDto stockItem)
		{
			var isCreated = await this.stockService.Create(stockItem);
			if (isCreated)
			{
				return new CreatedResult(
					new Uri(string.Format(this.stockCreateLocationFormat, stockItem.Id)),
					stockItem);
			}

			return new ConflictResult();
		}

		/// <summary>
		///   Read <see cref="StockItem" /> data by the given <paramref name="id" />.
		/// </summary>
		/// <param name="id">The <see cref="StockItem.Id" /> of the item.</param>
		/// <returns>An <see cref="OkObjectResult" /> containing the <see cref="StockItemDto" /> or <see cref="NotFoundResult" />.</returns>
		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> ReadById([FromRoute] Guid id)
		{
			if (id == Guid.Empty)
			{
				return null;
			}

			var stockItem = await this.stockService.ReadById(id);
			if (stockItem is null)
			{
				return new NotFoundResult();
			}

			return new OkObjectResult(stockItem);
		}

		/// <summary>
		///   Update <see cref="StockItemDto.InStock" /> for item with <paramref name="id" /> by adding the given
		///   <paramref name="delta" />.
		/// </summary>
		/// <param name="id">The <see cref="StockItemDto.Id" /> of the item to be updated.</param>
		/// <param name="delta">The difference of the <see cref="StockItemDto.InStock" /> to be added.</param>
		/// <returns>
		///   A <see cref="NotFoundResult" /> if the <see cref="StockItemDto.Id" /> is unknown, an
		///   <see cref="OkObjectResult" /> if the update operation succeeded and a <see cref="ConflictResult" /> if less items in
		///   storage than requested.
		/// </returns>
		[HttpPut]
		[Route("{id:guid}/{delta:int}")]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromRoute] int delta)
		{
			var (stockItem, isUpdated) = await this.stockService.Update(id, delta);
			// stock item does not exist
			if (stockItem is null)
			{
				return new NotFoundResult();
			}

			// less items in stock than requested
			if (isUpdated)
			{
				return new OkObjectResult(stockItem);
			}

			// update successful
			return new ConflictObjectResult(stockItem);
		}
	}
}