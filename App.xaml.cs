using CRM_App.Data;
using Application = Microsoft.Maui.Controls.Application;

namespace CRM_App;

public partial class App : Application
{
    public static DatabaseHelper DatabaseHelper { get; private set; }

	public App(DatabaseHelper databaseHelper)
	{
		InitializeComponent();

        MainPage = new AppShell();
        DatabaseHelper = databaseHelper;
    }

    protected override void OnStart()
    {
        base.OnStart();
        DatabaseHelper.InitializeDatabase().Wait();

        //verificare daca user este logat
        if (Data.AuthenticationManager.IsLoggedIn)
        {
            ((AppShell)MainPage).ShowFlyoutMenu();
            Shell.Current.GoToAsync("///MainPage").Wait();
        }
        else
        {
            ((AppShell)MainPage).HideFlyoutMenu();
            Shell.Current.GoToAsync("///LoginPage").Wait();
        }
    }
}
