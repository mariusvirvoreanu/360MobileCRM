using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;
using CRM_App.Data;
using CRM_App.Views;

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

        // Check if the user is logged in
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
