using CRM_App.Data;
using CRM_App.Models;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;

namespace CRM_App.Views;

public partial class AddCustomerPage : ContentPage
{
	public AddCustomerPage()
	{
		InitializeComponent();
	}

    private async void OnAddCustomerClicked(object sender, EventArgs e)
    {
        bool isValid = ValidateFields();

        if (!isValid)
        {
            return;
        }

        var customer = new Customer
        {
            Name = NameEntry.Text,
            Email = EmailEntry.Text,
            Phone = PhoneEntry.Text,
            Address = AddressEntry.Text
        };

        var result = await DatabaseHelper.AddCustomerAsync(customer);
        if (result == 0)
        {
            await DisplayAlert("Eroare", "Clientul nu a fost adaugat!", "OK");
        }
        else
        {
            await DisplayAlert("Info", "Client adaugat cu succes", "OK");
            // Navigate back to MainPage
            await Shell.Current.GoToAsync("///MainPage");
        } 
    }
    private bool ValidateFields()
    {
        bool isValid = true;

        // Validate Name
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            NameError.Text = "Completati numele!";
            NameError.IsVisible = true;
            isValid = false;
        }
        else
        {
            NameError.IsVisible = false;
        }

        // Validate Email
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || !IsValidEmail(EmailEntry.Text))
        {
            EmailError.Text = "Completati o adresa de email valida!";
            EmailError.IsVisible = true;
            isValid = false;
        }
        else
        {
            EmailError.IsVisible = false;
        }

        // Validate Phone
        if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
        {
            PhoneError.Text = "Completati numarul de telefon!";
            PhoneError.IsVisible = true;
            isValid = false;
        }
        else
        {
            PhoneError.IsVisible = false;
        }

        // Validate Address
        if (string.IsNullOrWhiteSpace(AddressEntry.Text))
        {
            AddressError.Text = "Completati adresa!";
            AddressError.IsVisible = true;
            isValid = false;
        }
        else
        {
            AddressError.IsVisible = false;
        }

        return isValid;
    }
    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}