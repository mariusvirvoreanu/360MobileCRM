using CRM_App.Data;
using CRM_App.ViewModels;
using CRM_App.Views;
using Microsoft.Extensions.Logging;
using SQLite;

namespace CRM_App;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
        //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CRM.db3");
        //builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<DatabaseHelper>(s, dbPath));
        builder.Services.AddSingleton<DatabaseHelper>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<UserPage>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<UserViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();

        return builder.Build();
	}
}
