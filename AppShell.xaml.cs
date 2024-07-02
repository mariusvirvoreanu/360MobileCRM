using CRM_App.Views;
using System.Windows.Input;

namespace CRM_App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        //Inregistrare rute pagini
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

        //Setare BindingContext pentru command bindings
        BindingContext = this; 
        //Comanda Logout
        LogoutCommand = new Command(async () => await LogoutAsync());
        //initial ascundem meniul Flyout
        HideFlyoutMenu();
    }

    public ICommand LogoutCommand { get; }

    private async Task LogoutAsync()
    {
        //Clear flag IsLoggedIn 
        Data.AuthenticationManager.IsLoggedIn = false;
        //Ascundere meniu Flyout
        HideFlyoutMenu();
        //Navigare pagina Login
        await Shell.Current.GoToAsync("///LoginPage");
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //Navigare la pagina Login dupa ce AppShell este initializata complet
        await AppShell.NavigateToLoginPage();
    }
    private static async Task NavigateToLoginPage()
    {
        //Verificare daca Pagina login este deja in navigation stack
        if (!Shell.Current.Navigation.NavigationStack.Any(page => page is LoginPage))
        {
            //route absoluta => ///
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
    public void ShowFlyoutMenu()
    {
        //Setare FlyoutBehavior = enabled
        this.FlyoutBehavior = FlyoutBehavior.Flyout;
        //FlyoutItems=> visible
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
        //Setare FlyoutBehavior = disabled
        this.FlyoutBehavior = FlyoutBehavior.Disabled;
        //FlyoutItems = hidden
        foreach (var item in this.Items)
        {
            if (item is FlyoutItem flyoutItem)
            {
                flyoutItem.IsVisible = false;
            }
        }
    }

}
