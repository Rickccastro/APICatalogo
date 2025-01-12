using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"Executando OnActionExecuting{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation("////////////////////////////////");
        _logger.LogInformation($"ModelState:{ context.ModelState.IsValid}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"Executando OnActionExecuted{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation("////////////////////////////////");
        _logger.LogInformation($"ModelState:{context.ModelState.IsValid}");
    }
}

