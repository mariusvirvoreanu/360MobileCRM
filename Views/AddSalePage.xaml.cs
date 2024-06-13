using CRM_App.Data;
using CRM_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace CRM_App.Views;

[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class AddSalePage : ContentPage
{
    private int _customerId;
    public int CustomerId
    {
        get => _customerId;
        set
        {
            _customerId = value;
            LoadCustomer(value);
        }
    }

    public Customer customer;
    public AddSalePage()
	{
		InitializeComponent();
        LoadProductsAndStatuses();
    }
    private async void LoadCustomer(int customerId)
    {
        customer = await DatabaseHelper.GetCustomerByIdAsync(customerId);
        BindingContext = customer;
        NameLabel.Text = customer.Name;
    }

    private async void LoadProductsAndStatuses()
    {
        List<Product> products = await DatabaseHelper.GetAllProductsAsync();
        foreach (Product product in products)
        {
            ProductNamePicker.Items.Add(product.Name);
        }
        List<Status> statuses = await DatabaseHelper.GetAllStatusesAsync();
        foreach (Status status in statuses) 
        {
            StatusPicker.Items.Add(status.StatusName);
        }
        //default = 'in progress'
        StatusPicker.SelectedIndex = 0;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        bool isValid = ValidateFields();

        if (!isValid)
        {
            return;
        }

        string selectedProductName = ProductNamePicker.SelectedItem?.ToString();
        var product = await DatabaseHelper.GetProductByNameAsync(selectedProductName);
        if (product == null)
        {
            await DisplayAlert("Eroare", "Nu am gasit id produs", "Ok");
            return;
        }
        int productId = product.ProductID;

        string selectedStatus = StatusPicker.SelectedItem?.ToString();
        
        Sale newSale = new Sale
        {
            CustomerID = _customerId,
            ProductID = productId,
            TotalAmount = Convert.ToDecimal(TotalAmountEntry.Text),
            UserID = AuthenticationManager.LoggedUser.UserID,
            Status = selectedStatus,
            Description = DescriptionEntry.Text
        };

        var result = await DatabaseHelper.AddSaleAsync(newSale);
        if (result == 0) 
        {
            await DisplayAlert("Eroare", "Vanzarea nu a fost adaugata", "OK");
        }
        else
        {
            await DisplayAlert("Info", "Vanzare adaugata cu succes", "OK");
            // Navigate back to the client page
            await Navigation.PopAsync();
        }
    }

    private bool ValidateFields()
    {
        bool isValid = true;
        // Validate Product
        if (string.IsNullOrWhiteSpace(ProductNamePicker.SelectedItem?.ToString()))
        {
            ProductError.Text = "Selectati Produsul!";
            ProductError.IsVisible = true;
            isValid = false;
        }
        else
        {
            ProductError.IsVisible = false;
        }
        //Validate TotalAmountEntry
        if(double.TryParse(TotalAmountEntry.Text.Trim(), out double totalAmount))
        {
            AmountError.IsVisible = false;
        }
        else
        {
            AmountError.Text = "Completati suma corect!";
            AmountError.IsVisible = true;
            isValid = false;
        }
        // Validate Description
        if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
            DescriptionError.Text = "Completati descrierea!";
            DescriptionError.IsVisible = true;
            isValid = false;
        }
        else
        {
            DescriptionError.IsVisible = false;
        }

        return isValid;
    }


}