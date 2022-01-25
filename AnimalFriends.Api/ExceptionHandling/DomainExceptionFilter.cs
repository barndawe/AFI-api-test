using System.Net;
using AnimalFriends.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AnimalFriends.Api.ExceptionHandling;

public class DomainExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is DomainException domainException)
        {
            context.Result = new JsonResult(new { Message = domainException.Message })
            {
                StatusCode = (int)domainException.StatusCode
            };
            
            context.ExceptionHandled = true;
        }
        else if (context.Exception is not null)
        {
            context.Result = new JsonResult(new { Message = context.Exception.Message })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            
            context.ExceptionHandled = true;
        }
    }
}