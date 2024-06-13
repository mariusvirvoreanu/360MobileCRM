using CRM_App.Data;
using CRM_App.Models;

namespace CRM_App.Views;

[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class UpdateCustomerPage : ContentPage
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

    public UpdateCustomerPage()
    {
        InitializeComponent();
    }
    private async void LoadCustomer(int customerId)
    {
        var customer = await DatabaseHelper.GetCustomerByIdAsync(customerId);
        BindingContext = customer;

        NameEntry.Text = customer.Name;
        EmailEntry.Text = customer.Email;
        PhoneEntry.Text = customer.Phone;
        AddressEntry.Text = customer.Address;
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        var customer = new Customer
        {
            CustomerID = _customerId,
            Name = NameEntry.Text,
            Email = EmailEntry.Text,
            Phone = PhoneEntry.Text,
            Address = AddressEntry.Text
        };

        await DatabaseHelper.UpdateCustomerAsync(customer);
        //Navigate back to customer details page
        await Shell.Current.Navigation.PopAsync();
    }
}