namespace api.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    readonly ILogger<BaseApiController> _logger;

    public BaseApiController(ILogger<BaseApiController> logger)
    {
        _logger = logger;
    }

    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public BaseApiResult Error()
    {
        Response.StatusCode = 404;
        ErrorModel error = new ErrorModel();

        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (context is null)
        {
            error.Message = "Not found!";
            return new BaseApiResult { IsSuccess = false, Error = error };
        }
        var foo = context.Error.GetType().FullName;

        switch (foo)
        {
            case nameof(ServiceException):
                Response.StatusCode = 400;
                error.Message = context.Error.Message;
                _logger.LogWarning(error.Message);
                break;
            case nameof(ApiValidationException):
                Response.StatusCode = 400;
                error.Message = string.Join("; ", ((ApiValidationException)context.Error).Failures);
                _logger.LogWarning(error.Message);
                break;
            default:
                Response.StatusCode = 500;
                error.Message = context.Error.Message;
                _logger.LogError((Exception)context.Error, error.Message + " " + context.Error.InnerException);
                break;
        }

        return new BaseApiResult { IsSuccess = false, Error = error };
    }
}
