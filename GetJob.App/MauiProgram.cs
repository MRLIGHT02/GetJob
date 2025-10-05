using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using GetJob.ServiceContracts.DTOs;


namespace GetJob.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient("CareerLinker", config =>
            {
                var url = "https://fphng75t-7143.inc1.devtunnels.ms/";
                config.BaseAddress= new Uri(url);
            });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
