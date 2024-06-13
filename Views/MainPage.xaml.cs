using CRM_App.Data;
using CRM_App.Models;
using CRM_App.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
        //check user to be logged
        if (AuthenticationManager.LoggedUser.UserID==0)
        {
            await DisplayAlert("Eroare", "Nu sunteti logat!", "Ok");
            await Shell.Current.GoToAsync("///LoginPage");
        }
        // Show the flyout menu when the MainPage appears
        ShowFlyoutMenu();
        //Search Client empty data
        CustomerSearchBar.Text = string.Empty;
        _customers.Clear();
        //show admin button
        if (AuthenticationManager.LoggedUser.Role == "admin")
        {
            AdminPageButton.IsVisible = true;
        }
    }
    private void ShowFlyoutMenu()
    {
        // Activate the flyout menu if the Shell's navigation stack contains only one page (i.e., the MainPage)
        if (Shell.Current.Navigation.NavigationStack.Count == 1)
        {
            // Open the flyout menu by setting the FlyoutIsPresented property to true
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        }
    }

}