using CRM_App.Views;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace CRM_App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for pages
        //Routing.RegisterRoute("///LoginPage", typeof(LoginPage));
        //Routing.RegisterRoute("///MainPage", typeof(MainPage));
        Routing.RegisterRoute("///RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
        Routing.RegisterRoute(nameof(AddCustomerPage), typeof(AddCustomerPage));
        Routing.RegisterRoute(nameof(CustomerDetailPage), typeof(CustomerDetailPage));
        Routing.RegisterRoute(nameof(UpdateCustomerPage), typeof(UpdateCustomerPage));
        Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
        Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
        Routing.RegisterRoute(nameof(UpdateProductPage), typeof(UpdateProductPage));
        Routing.RegisterRoute(nameof(AddSalePage), typeof(AddSalePage));
        Routing.RegisterRoute(nameof(UpdateSalePage), typeof(UpdateSalePage));
        Routing.RegisterRoute(nameof(UserSalesPage), typeof(UserSalesPage));
        Routing.RegisterRoute(nameof(AddRequestPage), typeof(AddRequestPage));
        Routing.RegisterRoute(nameof(CustomerRequestsPage), typeof(CustomerRequestsPage));
        Routing.RegisterRoute(nameof(UpdateRequestPage), typeof(UpdateRequestPage));
        Routing.RegisterRoute(nameof(UserRequestsPage), typeof(UserRequestsPage));

        //Set the BindingContext for command bindings
        BindingContext = this; 
        //Logout Command
        LogoutCommand = new Command(async () => await LogoutAsync());
        // Hide the Flyout menu initially
        HideFlyoutMenu();
    }

    public ICommand LogoutCommand { get; }

    private async Task LogoutAsync()
    {
        // Clear IsLoggedIn flag
        Data.AuthenticationManager.IsLoggedIn = false;
        // Hide the Flyout menu
        HideFlyoutMenu();
        // Navigate to LoginPage
        await Shell.Current.GoToAsync("///LoginPage");
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Navigate to LoginPage after the AppShell is fully initialized
        await AppShell.NavigateToLoginPage();
    }
    private static async Task NavigateToLoginPage()
    {
        // Check if LoginPage is already in the navigation stack
        if (!Shell.Current.Navigation.NavigationStack.Any(page => page is LoginPage))
        {
            // Navigate to LoginPage using absolute route ///
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
    public void ShowFlyoutMenu()
    {
        // Set FlyoutBehavior to enabled
        this.FlyoutBehavior = FlyoutBehavior.Flyout;
        // Enable all FlyoutItems
        foreach (var item in this.Items)
        {
            if (item is FlyoutItem flyoutItem)
            {
                flyoutItem.IsVisible = true;
            }
        }
    }
    public void HideFlyoutMenu()
    {
        // Set FlyoutBehavior to disabled
        this.FlyoutBehavior = FlyoutBehavior.Disabled;
        // Hide all FlyoutItems
        foreach (var item in this.Items)
        {
            if (item is FlyoutItem flyoutItem)
            {
                flyoutItem.IsVisible = false;
            }
        }
    }

}
