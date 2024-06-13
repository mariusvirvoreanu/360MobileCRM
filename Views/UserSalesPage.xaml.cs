using CRM_App.Data;
using CRM_App.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_App.Views;

public partial class UserSalesPage : ContentPage
{
    public UserSalesPage()
	{
        InitializeComponent();
        //load username
        Username.Text = AuthenticationManager.LoggedUser.Username;
        // Load sales for the selected user
        LoadSales();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSales();
    }
    private async void LoadSales()
    {
        // Fetch sales for the selected user from the database
        var sales = await DatabaseHelper.GetSalesReportForUser(AuthenticationManager.LoggedUser.UserID);
        // Bind sales to the ListView
        SalesCollectionView.ItemsSource = sales;
    }
    private async void OnUpdateSaleClicked(object sender, EventArgs e)
    {
        // Get the sale ID from the command parameter
        int saleId = (int)((Button)sender).CommandParameter;
        // Navigate to update sale page passing the sale ID
        await Navigation.PushAsync(new UpdateSalePage(saleId));
    }
    private async void OnDeleteSaleClicked(object sender, EventArgs e)
    {
        // Get the sale ID from the command parameter
        int saleId = (int)((Button)sender).CommandParameter;
        // Confirm deletion
        bool confirmDelete = await DisplayAlert("Atentie!", "Sunteti sigur ca doriti stergerea acestui produs din lista?", "Da", "Nu");
        if (confirmDelete)
        {
            var result = await DatabaseHelper.DeleteSaleAsync(saleId);
            if (result == 0)
            {
                await DisplayAlert("Eroare", "Nu am putut sterge produsul", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Produsul a fost sters", "Ok");
                // Reload sales for the customer
                LoadSales();
            }
        }
    }
    private void OnSaleSelected(object sender, SelectedItemChangedEventArgs e)
    { 
        // Deselect the selected item
        ((ListView)sender).SelectedItem = null;
    }

}