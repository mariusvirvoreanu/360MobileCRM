using CRM_App.Data;
using CRM_App.Models;

namespace CRM_App.Views;

[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class CustomerDetailPage : ContentPage
{
    private int _customerId;
    public int CustomerId
    {
        get => _customerId;
        set
        {
            _customerId = value;
            LoadSales(value);
            LoadCustomer(value);
        }
    }

    public CustomerDetailPage()
    {
        InitializeComponent();
    }

    private async void LoadCustomer(int customerId)
    {
        var customer = await DatabaseHelper.GetCustomerByIdAsync(customerId);
        BindingContext = customer;

        NameLabel.Text = customer.Name;
        EmailLabel.Text = customer.Email;
        PhoneLabel.Text = customer.Phone;
        AddressLabel.Text = customer.Address;
    }
    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(UpdateCustomerPage)}?CustomerId={CustomerId}");
    }
    private async void OnAddSaleClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddSalePage)}?CustomerId={CustomerId}");
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Info", "Sunteti sigur ca doriti sa stergeti acest client?", "Da", "Nu");
        if (result)
        {
            //verificare: clientul are vanzari sau solicitari ?
            var sales = await DatabaseHelper.GetAllSalesForCustomerAsync(CustomerId);
            if (sales.Count>0) 
            {
                if (AuthenticationManager.LoggedUser.Role=="admin") {
                   var deleteSales =  await DisplayAlert("Antentie!", "Acest client are vanzari!" 
                       + Environment.NewLine + "Stergem toate vanzarile pentru client?", "Da", "Nu");
                    if (!deleteSales)
                    {
                        return;
                    }
                    int rowsDeleted = await DatabaseHelper.DeleteSalesAsync(sales);
                    if (rowsDeleted == 0)
                    {
                        await DisplayAlert("Eroare", "Nu am putut sterge vanzarile clientului", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Info", $"Vanzarile pentru clientul: {NameLabel.Text} au fost sterse", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Atentie!", "Acest client are vanzari!", "Ok");
                    return;
                }
            }
            var requests = await DatabaseHelper.GetAllRequestsForCustomerAsync(CustomerId);
            if (requests.Count > 0)
            {
                if (AuthenticationManager.LoggedUser.Role == "admin")
                {
                    var deleteRequests = await DisplayAlert("Antentie!", "Acest client are solicitari!"
                        + Environment.NewLine + "Stergem toate solicitarile pentru client?", "Da", "Nu");
                    if (!deleteRequests)
                    {
                        return;
                    }
                    //prima data stergem comentariile
                    foreach (Request request in requests)
                    {
                        await DatabaseHelper.DeleteCommentsAsync(request.RequestID);
                    }
                    //stergere solicitari
                    int rowsDeleted = await DatabaseHelper.DeleteRequestsAsync(requests);
                    if (rowsDeleted == 0)
                    {
                        await DisplayAlert("Eroare", "Nu am putut sterge solicitarile clientului", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Info", $"Solicitarile pentru clientul: {NameLabel.Text} au fost sterse", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Atentie!", "Acest client are solicitari!", "Ok");
                    return;
                }
            }

            var result2 = await DatabaseHelper.DeleteCustomerAsync(CustomerId);
            if (result2==0)
            {
                await DisplayAlert("Eroare", "Clientul nu a putut fi sters", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Clientul a fost sters", "Ok");
                await Shell.Current.GoToAsync("///MainPage");
            }
        }
    }
    private async void LoadSales(int customerId)
    { 
        List<SaleReportLine> sales = await DatabaseHelper.GetSalesReportForCustomer(customerId);
        SalesCollectionView.ItemsSource = sales;
    }
    private async void OnUpdateSaleClicked(object sender, EventArgs e)
    {
        //sale ID de la command parameter
        int saleId = (int)((Button)sender).CommandParameter;
        //Navigare pagina update vanzare pt saleID
        await Navigation.PushAsync(new UpdateSalePage(saleId));
    }
    private async void OnDeleteSaleClicked(object sender, EventArgs e)
    {
        //saleID de la command parameter
        int saleId = (int)((Button)sender).CommandParameter;
        //Confirmare stergere
        bool confirmDelete = await DisplayAlert("Atentie!", "Sunteti sigur ca doriti stergerea acestui produs din lista?", "Da", "Nu");
        if (confirmDelete)
        {
            var result = await DatabaseHelper.DeleteSaleAsync(saleId);
            if (result==0)
            {
                await DisplayAlert("Eroare", "Nu am putut sterge produsul", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Produsul a fost sters", "Ok");
                //Reincarcare vanzari client
                LoadSales(_customerId);
            } 
        }
    }
    private void OnSaleSelected(object sender, SelectedItemChangedEventArgs e)
    {
        //Deselectare item selectat
        ((ListView)sender).SelectedItem = null;
    }
    private async void OnAddRequestClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddRequestPage)}?CustomerId={_customerId}");
    }
    private async void OnViewRequestsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(CustomerRequestsPage)}?CustomerId={_customerId}");
    }
}