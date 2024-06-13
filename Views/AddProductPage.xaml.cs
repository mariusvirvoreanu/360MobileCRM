using CRM_App.Data;
using CRM_App.Models;

namespace CRM_App.Views;

public partial class AddProductPage : ContentPage
{
	public AddProductPage()
	{
		InitializeComponent();
	}

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        bool isValid = ValidateFields();

        if (!isValid)
        {
            return;
        }

        var product = new Product
        {
            Name = NameEntry.Text,
            Description = DescriptionEntry.Text
        };

        var result = await DatabaseHelper.AddProductAsync(product);
        if (result == 0)
        {
            await DisplayAlert("Eroare", "Produsul nu a fost adaugat", "Ok");
        }
        else
        {
            await DisplayAlert("Info", "Produs adaugat cu succes", "Ok");
            //go back to admin page
            await Shell.Current.Navigation.PopAsync();
        }
    }

    private bool ValidateFields()
    {
        bool isValid = true;
        // Validate Name
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            NameError.Text = "Completati numele produsului!";
            NameError.IsVisible = true;
            isValid = false;
        }
        else
        {
            NameError.IsVisible = false;
        }
        // Validate Description
        if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
            DescriptionError.Text = "Completati descrierea produsului!";
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