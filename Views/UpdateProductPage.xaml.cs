using CRM_App.Data;
using CRM_App.Models;

namespace CRM_App.Views;

[QueryProperty(nameof(ProductId), nameof(ProductId))]
public partial class UpdateProductPage : ContentPage
{
    private int _productId;
    private Product _product;
    public int ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            LoadProduct(value);
        }
    }

    public UpdateProductPage()
    {
        InitializeComponent();
    }

    private async void LoadProduct(int productId)
    {
        _product = await DatabaseHelper.GetProductByIdAsync(productId);
        if (_product != null)
        {
            NameEntry.Text = _product.Name;
            DescriptionEntry.Text = _product.Description;
        }
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_product != null)
        {
            _product.Name = NameEntry.Text;
            _product.Description = DescriptionEntry.Text;
            var result = await DatabaseHelper.UpdateProductAsync(_product);
            if (result==0)
            {
                await DisplayAlert("Eroare", "Produsul nu a fost actualizat", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Produs actualizat", "Ok");
                //navigare pagina admin
                await Shell.Current.Navigation.PopAsync();
            }
            
        }
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_product != null)
        {
            var result = await DatabaseHelper.DeleteProductAsync(_product.ProductID);
            if (result==0)
            {
                await DisplayAlert("Eroare", "Produs nu a fost sters", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Produs a fost sters", "Ok");
                //navigare pagina admin
                await Shell.Current.Navigation.PopAsync();
            }
            
        }
    }
}