using CommunityToolkit.Maui;
using ICarus_Rental.Pages;
using ICarus_Rental.Pages.Authorization;
using ICarus_Rental.Pages.User;
using ICarus_Rental.ViewModels;
using Microsoft.Extensions.Logging;

namespace ICarus_Rental
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<SortPage>();
            builder.Services.AddSingleton<SortViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<CardsPage>();
            builder.Services.AddSingleton<CardsViewModel>();
            builder.Services.AddTransient<LocationPage>();
            builder.Services.AddTransient<LocationViewModel>();
            builder.Services.AddTransient<CarRentPage>();
            builder.Services.AddTransient<CarRentViewModel>();
            builder.Services.AddTransient<CarHistoryPage>();
            builder.Services.AddTransient<CarHistoryViewModel>();
            builder.Services.AddTransient<RentSummaryPage>();
            builder.Services.AddTransient<RentSummaryViewModel>();
            builder.Services.AddTransient<RentInfoPage>();
            builder.Services.AddTransient<RentInfoViewModel>();
            builder.Services.AddTransient<CardAddPage>();
            builder.Services.AddTransient<CardAddViewModel>();
            builder.Services.AddTransient<IncidentPage>();
            builder.Services.AddTransient<IncidentViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
