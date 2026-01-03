using Microsoft.JSInterop;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RCLAPI.Extensions;
using RCLAPI.Services;

namespace MyMEDIA.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // HTTP + JWT
        builder.Services.AddSingleton<TokenService>(sp =>
        {
            var jsRuntime = sp.GetService<IJSRuntime>();
            return new TokenService(jsRuntime!);
        });
        builder.Services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("https://bggdp96v-5063.uks1.devtunnels.ms/")
        });

        builder.Services.AddRCLApiServices("https://bggdp96v-5063.uks1.devtunnels.ms/");

        return builder.Build();
    }
}