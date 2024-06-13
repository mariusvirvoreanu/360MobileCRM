using CRM_App.Data;
using CRM_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml;

namespace CRM_App.Views;

public partial class UpdateSalePage : ContentPage
{
    private readonly int _saleId;
    public UpdateSalePage(int saleId)
    {
        InitializeComponent();
        _saleId = saleId;
        // Load the sale details
        LoadStatuses();
        LoadSaleDetails();
        
    }
    private async void LoadSaleDetails()
    {
        var lista = await DatabaseHelper.GetAllSalesForCustomerAsync(1);
        foreach (var item in lista)
        {
            Debug.WriteLine(item.ToString());
        }


        Sale sale = DatabaseHelper.GetSaleById(_saleId);
        if (sale != null)
        {
            string customerName = DatabaseHelper.GetCustomerByIdAsync(sale.CustomerID).Result.Name;
            string productName = DatabaseHelper.GetProductByIdAsync(sale.ProductID).Result.Name;

            NameLabel.Text = customerName;
            ProductNamelabel.Text = productName;
            AmountLabel.Text = sale.TotalAmount.ToString();
            StatusPicker.SelectedItem = sale.Status;
            DescriptionEntry.Text = sale.Description;
        }
        else
        {
            await DisplayAlert("Eroare", "Vanzarea nu a fost gasita", "Ok");
            await Navigation.PopAsync();
        }
    }
    private async void LoadStatuses()
    {
        List<Status> statuses = await DatabaseHelper.GetAllStatusesAsync();
        foreach (Status status in statuses)
        {
            StatusPicker.Items.Add(status.StatusName);
        }
    }
    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        Sale sale = DatabaseHelper.GetSaleById(_saleId);
        if (sale != null)
        {
            if (string.IsNullOrEmpty(DescriptionEntry.Text))
            {
                DescriptionError.Text = "Introduceti descrierea";
                DescriptionError.IsVisible = true;
                return;
            }

            sale.Status = StatusPicker.SelectedItem?.ToString();
            sale.Description = DescriptionEntry.Text;

            var result = await DatabaseHelper.UpdateSaleAsync(sale);

            if (result == 0)
            {
                await DisplayAlert("Eroare", "Nu am putut actualiza vanzarea", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Vanzarea a fost actualizata", "Ok");
                //Navigate back to the customer detail page
                await Navigation.PopAsync();
            }
        }
        else
        {
            await DisplayAlert("Eroare", "Vanzarea nu a fost gasita", "Ok");
        }
    }
}