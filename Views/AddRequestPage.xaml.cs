using CRM_App.Data;
using CRM_App.Models;

namespace CRM_App.Views;

[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class AddRequestPage : ContentPage
{
    private int _customerId;

    public int CustomerId
    {
        get => _customerId;
        set
        {
            _customerId = value;
        }
    }
    public AddRequestPage()
	{
		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        CustomerName.Text = DatabaseHelper.GetCustomerByIdAsync(_customerId).Result.Name;
        StatusPicker.SelectedIndex = 0;
    }

    private async void OnAddRequestClicked(object sender, EventArgs e)
    {
        bool isValid = ValidateFields();
        if (!isValid)
        {
            return;
        }
        var request = new Request
        {
            CustomerID = _customerId,
            UserID = AuthenticationManager.LoggedUser.UserID,
            Description = DescriptionEntry.Text,
            Status = StatusPicker.SelectedItem.ToString()
        };

        int result = await DatabaseHelper.AddRequestAsync(request);
        if (result == 0) 
        {
            await DisplayAlert("Eroare", "Solicitarea clientului nu a fost adaugata", "Ok");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Info", "Solicitarea clientului a fost adaugata cu success", "Ok");
            await Navigation.PopAsync();
        }
    }
    private bool ValidateFields()
    {
        bool isValid = true;
        // Validate Product
        if (string.IsNullOrWhiteSpace(StatusPicker.SelectedItem?.ToString()))
        {
            StatusPickerError.Text = "Selectati statusul!";
            StatusPickerError.IsVisible = true;
            isValid = false;
        }
        else
        {
            StatusPickerError.IsVisible = false;
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