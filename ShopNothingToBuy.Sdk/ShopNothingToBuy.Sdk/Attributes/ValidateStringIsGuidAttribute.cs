namespace ShopNothingToBuy.Sdk.Attributes
{
	using System;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;

	public class ValidateStringIsGuidAttribute : ActionFilterAttribute
	{
		private readonly string routeParameterName;

		public ValidateStringIsGuidAttribute(string routeParameterName)
		{
			this.routeParameterName = routeParameterName ?? throw new ArgumentNullException(nameof(routeParameterName));
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.ActionArguments.ContainsKey(this.routeParameterName))
			{
				var value = context.ActionArguments[this.routeParameterName] as string;
				if (string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out var guid) || guid == Guid.Empty)
				{
					context.Result = new BadRequestResult();
				}
			}
		}
	}
}