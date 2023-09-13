namespace API_Invisible_Watermark.Models;

public record Rgb
{
    public double[,] Alpha { get; set; }
    public double[,] Red { get; set; }
    public double[,] Green { get; set; }
    public double[,] Blue { get; set; }
}