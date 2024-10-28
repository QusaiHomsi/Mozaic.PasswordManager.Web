using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Common;
using Mozaic.PasswordManager.BL.Exceptions;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context )
    {
        if(context.Exception is not BusinessException)
        {
            LogingHelper.LogError(context.Exception.Message, context.Exception);
        }
      

        context.Result = new RedirectToActionResult("Error", "Home", new { message = "An unexpected error occurred." });
        context.ExceptionHandled = true;

    }
}
