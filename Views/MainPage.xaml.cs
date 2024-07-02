using CRM_App.Data;
using CRM_App.Models;
using System.Collections.ObjectModel;

namespace CRM_App.Views;

public partial class MainPage : ContentPage
{
    private ObservableCollection<Customer> _customers;

    public MainPage()
	{
        InitializeComponent();
        _customers = new ObservableCollection<Customer>();
        CustomerCollectionView.ItemsSource = _customers;
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        string query = CustomerSearchBar.Text;
        await SearchCustomersAsync(query);
    }
    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            _customers.Clear();
        }
    }
    private async Task SearchCustomersAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            _customers.Clear();
            return;
        }

        var customers = await DatabaseHelper.GetCustomersByNameAsync(query);
        _customers.Clear();
        foreach (var customer in customers)
        {
            _customers.Add(customer);
        }
        if (_customers.Count == 0)
        {
            await DisplayAlert("Info", "Nu am gasit niciun client cu acest nume sau email!", "Ok");
        }
    }
    private async void OnCustomerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Customer selectedCustomer)
        {
            await Shell.Current.GoToAsync($"{nameof(CustomerDetailPage)}?CustomerId={selectedCustomer.CustomerID}");
        }
    }
    private async void OnAddCustomerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddCustomerPage));
    }
    private async void OnAdminPageClicked(object sender, EventArgs e)
    {
        if (AuthenticationManager.LoggedUser.Role=="admin")
        {
            await Shell.Current.GoToAsync(nameof(AdminPage));
        }
        else
        {
            await DisplayAlert("Eroare", "Nu aveti drepturile necesare pentru a acesta acest meniu!", "Ok");
        }
        
    }
    private async void OnUserSalesPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserSalesPage());
    }
    private async void OnMyRequestsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(UserRequestsPage)}?UserId={AuthenticationManager.LoggedUser.UserID}");
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //verificam daca userul este logat
        if (AuthenticationManager.LoggedUser.UserID==0)
        {
            await DisplayAlert("Eroare", "Nu sunteti logat!", "Ok");
            await Shell.Current.GoToAsync("///LoginPage");
        }
        //afisam meniul flyout in pagina principala
        ShowFlyoutMenu();
        //Search Client = empty
        CustomerSearchBar.Text = string.Empty;
        _customers.Clear();
        //butonul pagina administrare este activ doar daca userul logat este admin
        if (AuthenticationManager.LoggedUser.Role == "admin")
        {
            AdminPageButton.IsVisible = true;
        }
    }
    private void ShowFlyoutMenu()
    {
        //activare meniu flyout menu
        if (Shell.Current.Navigation.NavigationStack.Count == 1)
        {
            //deschidere meniu flyout => setare proprietate FlyoutIsPresented = true
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        }
    }

}