using CRM_App.Data;
using CRM_App.Models;
using System.Text.RegularExpressions;
using CRM_App.Services;

namespace CRM_App.Views;

public partial class AddCustomerPage : ContentPage
{
    public AddCustomerPage()
    {
        InitializeComponent();
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        try
        {
            NameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PhoneEntry.Text = string.Empty;
            AddressEntry.Text = string.Empty;
        }
        catch
        {
        }
    }
    private async void OnSearchANAFClicked(object sender, EventArgs e)
    {
        try
        {
            string cui = await DisplayPromptAsync("CUI", "Introduceti CUI firma (valoare numerica)", "Ok", "Renunta");
            if (cui == null)
            {
                return;
            }
            if (cui.All(char.IsDigit) && cui.Length > 1)
            {
                var customer = await ApiAnaf.GetCustomerFromAnaf(cui);

                if (customer == null)
                {
                    await DisplayAlert("Eroare", "CUI-ul nu a fost gasit", "OK");
                }
                else
                {
                    NameEntry.Text = customer.Name;
                    PhoneEntry.Text = customer.Phone;
                    AddressEntry.Text = customer.Address;

                    EmailEntry.Focus();
                }
            }
            else
            {
                await DisplayAlert("Eroare", "CUI-ul introdus nu este corect", "OK");
            }
        }
        catch
        {
            await DisplayAlert("Eroare", "Eroare cautare client ANAF in baza CUI", "OK");
        }
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
            //Navigare la pagina principala
            await Shell.Current.GoToAsync("///MainPage");
        }
    }
    private bool ValidateFields()
    {
        bool isValid = true;

        //Validare Nume
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
        //Validare Email
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || !AddCustomerPage.IsValidEmail(EmailEntry.Text))
        {
            EmailError.Text = "Completati o adresa de email valida!";
            EmailError.IsVisible = true;
            isValid = false;
        }
        else
        {
            EmailError.IsVisible = false;
        }
        //Validare Telefon
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
        //Validare Adresa
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
    private static bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (AuthenticationManager.LoggedUser.Role == "admin")
        {
            adminMenu.IsVisible = true;
        }
    }

}