using System.Net;
using API_Invisible_Watermark.Exeptions;
using API_Invisible_Watermark.Models;


namespace API_Invisible_Watermark.Middlewares;

public class ExceptionHandling : IMiddleware
{
    private readonly ILogger _logger;

    public ExceptionHandling(ILogger<ExceptionHandling> logger)
    {
        _logger = logger;
    }
    


    private static async Task HandlingException(HttpContext context, Exception e)
    {
        var error = new ErrorResponse();

        switch (e)
        {
            case BadRequestException:
                error.Code = (int)HttpStatusCode.BadRequest;
                error.Status = HttpStatusCode.BadRequest.ToString();
                error.Message = e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                error.Code = (int)HttpStatusCode.NotFound;
                error.Status = HttpStatusCode.NotFound.ToString();
                error.Message = e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
        }
        
        await context.Response.WriteAsJsonAsync(error);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandlingException(context, e);
            _logger.LogError(e.Message);
        }
    }
}