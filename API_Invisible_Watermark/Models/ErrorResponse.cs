using System.Net;

namespace API_Invisible_Watermark.Models;

public class ErrorResponse
{
    public int Code { get; set; } = (int)HttpStatusCode.InternalServerError;
    public string Status { get; set; } = HttpStatusCode.InternalServerError.ToString();
    public string Message { get; set; } = "Internal Server Error";
}