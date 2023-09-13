using API_Invisible_Watermark.Models;
using API_Invisible_Watermark.Services.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API_Invisible_Watermark.Controllers.V1;

[ApiController]
[Route("/api/v1/invisible-watermark")]
public class InvisibleWatermarkController : ControllerBase
{
    private readonly IWatermarkService _watermarkService;

    public InvisibleWatermarkController(IWatermarkService watermarkService)
    {
        _watermarkService = watermarkService;
    }
    
    [HttpGet("/")]
    public Response<string> Index()
    {
        var response = new Response<string>()
        {
            Status = true,
            Message = "Succesfully",
            Data = "yey"
        };
        
        return response;
    }

    [EnableCors("AllowAll")]
    [HttpPost("create-watermarked-image")]
    public async Task<IActionResult> GetWatermarkedImage([FromForm]WatermarkRequest request)
    {
        var host = request.Host;
        var watermark = request.Watermark;
        var result = await _watermarkService.CreateWatermarkedImage(host,watermark);
        var response = new Response<ResultResponse>()
        {
            Status = true,
            Message = "Watermarked Image Successfully Created",
            Data = result
        };
        return Ok(response);
    }
}