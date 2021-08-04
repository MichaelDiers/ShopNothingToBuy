namespace ShopNothingToBuy.Sdk.Attributes
{
	using System;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;

	public class StringLengthAttribute : ActionFilterAttribute
	{
		private readonly int max;
		private readonly int min;
		private readonly string routeParameter;

		public StringLengthAttribute(string routeParameter, int min, int max)
		{
			if (string.IsNullOrWhiteSpace(routeParameter))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(routeParameter));
			}

			if (min < 0 || min > max)
			{
				throw new ArgumentException("Invalid argument", nameof(min));
			}

			if (max < min)
			{
				throw new ArgumentException("Invalid argument", nameof(max));
			}

			this.routeParameter = routeParameter;
			this.min = min;
			this.max = max;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.ActionArguments.ContainsKey(this.routeParameter))
			{
				var value = context.ActionArguments[this.routeParameter] as string;
				if (string.IsNullOrWhiteSpace(value) || value.Length < this.min || value.Length > this.max)
				{
					context.Result = new BadRequestResult();
				}
			}
			else
			{
				context.Result = new BadRequestResult();
			}
		}
	}
}