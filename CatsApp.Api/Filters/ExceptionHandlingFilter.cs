using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CatsApp.Application.Exceptions;

namespace CatsApp.Api.Filters;

internal class ExceptionHandlingFilter : ExceptionFilterAttribute
{
    private readonly ILogger _logger;
    public ExceptionHandlingFilter(ILogger logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        context.ExceptionHandled = TryHandleException(context, context.Exception);
    }

    private bool TryHandleException(ExceptionContext context, Exception? exception)
    {
        if (exception is CatNotFoundException catException)
        {
            context.Result = new ObjectResult(exception.Message) { StatusCode = StatusCodes.Status404NotFound };

            return true;
        }
        else
        {
            _logger.LogError(exception, "Unhandled exception with message {Message}", exception.Message);
            return false;
        }
    }
}
