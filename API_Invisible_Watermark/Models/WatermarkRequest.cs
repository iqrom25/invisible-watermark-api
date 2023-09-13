using System.ComponentModel.DataAnnotations;

namespace API_Invisible_Watermark.Models;

public record WatermarkRequest
{
    [Required]
    public IFormFile Host { get; set; }
    
    [Required]
    public IFormFile Watermark { get; set; }
};