using API_Invisible_Watermark.Services.Interface;

namespace API_Invisible_Watermark.Services;

public static class DepedencyInjection
{
    public static void AddApplication(this IServiceCollection service)
    {
        service.AddSingleton<IWatermarkService, WatermarkService>();
    }
}