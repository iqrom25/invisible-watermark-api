namespace API_Invisible_Watermark.Models;

public record ResultSvd
{
    public double[,] U { get; set; }
    public double[,] S { get; set; }
    public double[,] V { get; set; }
};