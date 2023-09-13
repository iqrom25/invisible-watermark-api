namespace API_Invisible_Watermark.Models;

public record ResultResponse
{
    public string WatermarkedImage { get; set; }
    
    public string HostDwt { get; set; }
    
    public string WatermarkDwt { get; set; }
    
    public double Psnr { get; set; }
};