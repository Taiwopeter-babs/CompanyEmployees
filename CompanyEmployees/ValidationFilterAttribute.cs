using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Acts as the validation for actions using HTTP POST and PUT methods
/// </summary>
public class ValidationFilterAttribute
{
    public ValidationFilterAttribute() { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.RouteData.Values["action"];
        var controllerName = context.RouteData.Values["controller"];

        var param = context.ActionArguments
            .SingleOrDefault(x => x.Value != null && x.Value.ToString().Contains("Dto")).Value;

        if (param is null)
        {
            context.Result = new BadRequestObjectResult($"Object is null. Controller: " +
                $"{controllerName}, action: {actionName}");
            return;
        }

        if (!context.ModelState.IsValid)
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }

    public void OnActionExecuted(ActionExecutingContext context) { }
}

