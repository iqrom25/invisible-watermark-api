

using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using API_Invisible_Watermark.Models;


namespace API_Invisible_Watermark.Utils;

public static class Image
{
    public static async Task<Bitmap> ToBitmap(this IFormFile file)
    {

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        return (Bitmap)System.Drawing.Image.FromStream(memoryStream);

    }
    
    public static Bitmap Resize(this Bitmap bitmap, int width, int height)
    {
        var filter = new ResizeNearestNeighbor(width,height);

        var newImage = filter.Apply(bitmap);

        return newImage;
    }
    
    public static Rgb GetRgb(this Bitmap bitmap)
    {


        var alpha = new double[bitmap.Width, bitmap.Height];
        var red = new double[bitmap.Width, bitmap.Height];
        var green = new double[bitmap.Width, bitmap.Height];
        var blue = new double[bitmap.Width, bitmap.Height];
        
        for (int i = 0; i < bitmap.Width; i++)
        {
            for (int j = 0; j < bitmap.Height; j++)
            {
                var color = bitmap.GetPixel(i, j);
                alpha[i, j] = color.A;
                red[i, j] = color.R;
                green[i, j] = color.G;
                blue[i, j] = color.B;
            }
        }

        var rgb = new Rgb
        {
            Alpha = alpha,
            Red = red,
            Green = green,
            Blue = blue
        };

        return rgb;
    }

    public static Bitmap ToBitmap(this Rgb historgram, int width, int height)
    {
        var image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        
        for (int i = 0; i < image.Width; i++)
        {
            for (int j = 0; j < image.Height; j++)
            {
                var color = Color.FromArgb((int)historgram.Alpha[i,j],(int)historgram.Red[i, j], (int)historgram.Green[i, j], (int)historgram.Blue[i, j]);
                // var color = Color.FromArgb(255,(int)historgram.Red[i, j], (int)historgram.Green[i, j], (int)historgram.Blue[i, j]);

                image.SetPixel(i,j,color);
            }
        }

        return image;
        
    }

    public static void Scaling(this Rgb histogram, int width, int height)
    {
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                histogram.Alpha[i, j] = Scale(0, 255, -1, 1, histogram.Alpha[i, j]);
                histogram.Red[i, j] = Scale(0, 255, -1, 1, histogram.Red[i, j]);
                histogram.Green[i, j] = Scale(0, 255, -1, 1, histogram.Green[i, j]);
                histogram.Blue[i, j] = Scale(0, 255, -1, 1, histogram.Blue[i, j]);
            }
        }
    }

    public static void Normalize(this Rgb histogram,int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                histogram.Alpha[i, j] = Scale(-1, 1, 0, 255, histogram.Alpha[i, j]);
                histogram.Red[i, j] = Scale(-1, 1, 0, 255, histogram.Red[i, j]);
                histogram.Green[i, j] = Scale(-1, 1, 0, 255, histogram.Green[i, j]);
                histogram.Blue[i, j] = Scale(-1, 1, 0, 255, histogram.Blue[i, j]);

            }
        }
    }
    
    public static void Normalize2(this Rgb histogram,int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // histogram.Red[i, j] = Scale(-1, 1, 0, 255, histogram.Red[i, j]);
                // histogram.Green[i, j] = Scale(-1, 1, 0, 255, histogram.Green[i, j]);
                // histogram.Blue[i, j] = Scale(-1, 1, 0, 255, histogram.Blue[i, j]);
                
                if (histogram.Red[i, j] > 255) histogram.Red[i, j] = 255;
                if (histogram.Green[i, j] > 255) histogram.Green[i, j] = 255;
                if (histogram.Blue[i, j] > 255) histogram.Blue[i, j] = 255;
                
                if (histogram.Red[i, j] < 0) histogram.Red[i, j] = 0;
                if (histogram.Green[i, j] < 0) histogram.Green[i, j] = 0;
                if (histogram.Blue[i, j] < 0) histogram.Blue[i, j] = 0;
            }
        }
    }
    
    public static string ToBase64(this Bitmap image)
    {
        using var stream = new MemoryStream();
        image.Save(stream,ImageFormat.Png);
        var byteImage = stream.ToArray();
        return Convert.ToBase64String(byteImage);
    }
    
    private static double Scale(double fromMin, double fromMax, double toMin, double toMax, double x)
    {
        if (fromMax - fromMin == 0) return 0;
        double value = (toMax - toMin) * (x - fromMin) / (fromMax - fromMin) + toMin;
        if (value > toMax)
        {
            value = toMax;
        }
        if (value < toMin)
        {
            value = toMin;
        }
        return value;
    }
    
    public static double Psnr(this Rgb result, Rgb original)
    {
        var m = result.Alpha.GetLength(0);
        var n = result.Alpha.GetLength(1);
        double r = 0, g = 0, b = 0;

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var tmp = original.Red[i,j] - result.Red[i,j];
                r += Math.Pow(tmp, 2);
            }
        }

        r /= (m * n);
        
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var tmp = original.Green[i,j] - result.Green[i,j];
                g += Math.Pow(tmp, 2);
            }
        }
        
        g /= (m * n);
        
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var tmp = original.Blue[i,j] - result.Blue[i,j];
                b += Math.Pow(tmp, 2);
            }
        }
        
        b /= (m * n);

        var mse = (r + g + b) / 3;
        var psnr = 20 * Math.Log10(255 / Math.Sqrt(mse));
        
        return Math.Round(psnr,2);
    }
}