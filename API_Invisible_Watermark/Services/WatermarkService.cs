using Accord.Math;
using API_Invisible_Watermark.Models;
using API_Invisible_Watermark.Services.Interface;
using API_Invisible_Watermark.Utils;

namespace API_Invisible_Watermark.Services;

public class WatermarkService:IWatermarkService
{
    public async Task<ResultResponse> CreateWatermarkedImage(IFormFile host, IFormFile watermark)
    {
        var hostBitmap = await host.ToBitmap();
        var hostBitmapWidth = hostBitmap.Width;
        var hostBitmapHeight = hostBitmap.Height;

        var watermarkBitmap = await watermark.ToBitmap();
        var watermarkWidth = watermarkBitmap.Width;
        var watermarkHeight = watermarkBitmap.Height;
        
        var histogramHost = hostBitmap.GetRgb();
        var histogramOriginal = hostBitmap.GetRgb();
        var histogramWatermark = watermarkBitmap.GetRgb();

        histogramHost.Scaling(hostBitmapWidth,hostBitmapHeight);
        histogramWatermark.Scaling(watermarkWidth,watermarkHeight);
        
        Dwt.FWT(histogramHost.Alpha,2);
        Dwt.FWT(histogramHost.Red,2);
        Dwt.FWT(histogramHost.Green,2);
        Dwt.FWT(histogramHost.Blue,2);
        
        Dwt.FWT(histogramWatermark.Alpha,2);
        Dwt.FWT(histogramWatermark.Red,2);
        Dwt.FWT(histogramWatermark.Green,2);
        Dwt.FWT(histogramWatermark.Blue,2);

        histogramHost.Normalize(hostBitmapWidth,hostBitmapHeight);
        histogramWatermark.Normalize(watermarkWidth,watermarkHeight);

        var hostDwt = histogramHost.ToBitmap(hostBitmapWidth,hostBitmapHeight).ToBase64();
        var watermarkDwt = histogramWatermark.ToBitmap(watermarkWidth,watermarkHeight).ToBase64();
        
        histogramHost.Scaling(hostBitmapWidth,hostBitmapHeight);
        histogramWatermark.Scaling(watermarkWidth,watermarkHeight);
        
        var LL2HostAlpha = Dwt.GetLl2(histogramHost.Alpha);
        var LL2HostRed = Dwt.GetLl2(histogramHost.Red);
        var LL2HostGreen = Dwt.GetLl2(histogramHost.Green);
        var LL2HostBlue = Dwt.GetLl2(histogramHost.Blue);
        
        var LL2WatermarkAlpha = Dwt.GetLl2(histogramWatermark.Alpha);
        var LL2WatermarkRed = Dwt.GetLl2(histogramWatermark.Red);
        var LL2WatermarkGreen = Dwt.GetLl2(histogramWatermark.Green);
        var LL2WatermarkBlue = Dwt.GetLl2(histogramWatermark.Blue);
        
        var svdHostAlpha = LL2HostAlpha.SingularValueDecomposition();
        var svdHostRed = LL2HostRed.SingularValueDecomposition();
        var svdHostGreen = LL2HostGreen.SingularValueDecomposition();
        var svdHostBlue = LL2HostBlue.SingularValueDecomposition();
        
        
        var newSingularAlpha = svdHostAlpha.S.Addition(LL2WatermarkAlpha);
        var newSingularRed = svdHostRed.S.Addition(LL2WatermarkRed);
        var newSingularGreen = svdHostGreen.S.Addition(LL2WatermarkGreen);
        var newSingularBlue = svdHostBlue.S.Addition(LL2WatermarkBlue);
        
        var decompositionNewSingularAlpha = newSingularAlpha.SingularValueDecomposition();
        var decompositionNewSingularRed = newSingularRed.SingularValueDecomposition();
        var decompositionNewSingularGreen = newSingularGreen.SingularValueDecomposition();
        var decompositionNewSingularBlue = newSingularBlue.SingularValueDecomposition();
        
        var newLL2HostAlpha = svdHostAlpha.U.Dot(decompositionNewSingularAlpha.S).Dot(svdHostAlpha.V.Transpose());
        var newLL2HostRed = svdHostRed.U.Dot(decompositionNewSingularRed.S).Dot(svdHostRed.V.Transpose());
        var newLL2HostGreen = svdHostGreen.U.Dot(decompositionNewSingularGreen.S).Dot(svdHostGreen.V.Transpose());
        var newLL2HostBlue = svdHostBlue.U.Dot(decompositionNewSingularBlue.S).Dot(svdHostBlue.V.Transpose());
        
        // Dwt.ChangeLl2(histogramHost.Alpha,newLL2HostAlpha);
        Dwt.ChangeLl2(histogramHost.Red,newLL2HostRed);
        Dwt.ChangeLl2(histogramHost.Green,newLL2HostGreen);
        Dwt.ChangeLl2(histogramHost.Blue,newLL2HostBlue);
        
        histogramHost.Normalize(hostBitmapWidth,hostBitmapHeight);
        
        histogramHost.Scaling(hostBitmapWidth,hostBitmapHeight);
        
        Dwt.IWT(histogramHost.Alpha,2);
        Dwt.IWT(histogramHost.Red,2);
        Dwt.IWT(histogramHost.Green,2);
        Dwt.IWT(histogramHost.Blue,2);
        
        histogramHost.Normalize(hostBitmapWidth,hostBitmapHeight);
        
        var watermarkedImage = histogramHost.ToBitmap(hostBitmapWidth,hostBitmapHeight);

        var result = new ResultResponse
        {
            WatermarkedImage = watermarkedImage.ToBase64(),
            HostDwt = hostDwt,
            WatermarkDwt = watermarkDwt,
            Psnr = histogramHost.Psnr(histogramOriginal)
        };
        return result;

    }

    public double[,] ExtractWatermark()
    {
        throw new NotImplementedException();
    }
}