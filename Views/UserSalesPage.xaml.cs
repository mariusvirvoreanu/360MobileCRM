using CRM_App.Data;

namespace CRM_App.Views;

public partial class UserSalesPage : ContentPage
{
    public UserSalesPage()
	{
        InitializeComponent();
        //username
        Username.Text = AuthenticationManager.LoggedUser.Username;
        //incarcare vanzari
        LoadSales();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSales();
    }
    private async void LoadSales()
    {
        //vanzarile utilizatorului logat
        var sales = await DatabaseHelper.GetSalesReportForUser(AuthenticationManager.LoggedUser.UserID);
        //Bind vanzari la ListView
        SalesCollectionView.ItemsSource = sales;
    }
    private async void OnUpdateSaleClicked(object sender, EventArgs e)
    {
        //saleID din command parameter
        int saleId = (int)((Button)sender).CommandParameter;
        //navigare pagina update vanzare aferenta saleID
        await Navigation.PushAsync(new UpdateSalePage(saleId));
    }
    private async void OnDeleteSaleClicked(object sender, EventArgs e)
    {
        //saleID din command parameter
        int saleId = (int)((Button)sender).CommandParameter;
        //Confirmare stergere
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
                //reincarcare vanzari client
                LoadSales();
            }
        }
    }
    private void OnSaleSelected(object sender, SelectedItemChangedEventArgs e)
    { 
        //deselectare item selectat
        ((ListView)sender).SelectedItem = null;
    }

}