using System.Text.Json.Serialization;
using API_Invisible_Watermark.Middlewares;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;


namespace API_Invisible_Watermark.Extensions;

public static class ServiceExtensions
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Invisible Watermark With DWT & SVD",
                Description = "API yg bertugas untuk menyispkan gambar watermak ke dalam gambar host dengan menggunakan metode DWT & SVD",
                Contact = new OpenApiContact
                {
                    Name = "Muhammad Iqrom",
                    Email = "muhammad.prasetyo@ocbcnisp.com",
                    Url = new Uri("https://github.com/iqrom25"),
                }
            });
        });
    }

    public static void AddControllersExtension(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options => 
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        
        services.Configure<FormOptions>(o =>  
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = long.MaxValue;
            o.MultipartBoundaryLengthLimit = int.MaxValue;
            o.MultipartHeadersCountLimit = int.MaxValue;
            o.MultipartHeadersLengthLimit = int.MaxValue;
        });
        
        services.Configure<IISServerOptions>(o =>
        {
            o.MaxRequestBodySize = int.MaxValue;
        });
        
        services.Configure<KestrelServerOptions>(o =>
        {
            o.Limits.MaxRequestBodySize = int.MaxValue;
        });
    }

    public static void AddCorsExtension(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    public static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandling>();
    }
}