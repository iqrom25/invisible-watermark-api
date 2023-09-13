using System.Drawing;
using API_Invisible_Watermark.Models;

namespace API_Invisible_Watermark.Services.Interface;

public interface IWatermarkService
{
    Task<ResultResponse> CreateWatermarkedImage(IFormFile host, IFormFile watermark);
    
    double[,] ExtractWatermark();
}