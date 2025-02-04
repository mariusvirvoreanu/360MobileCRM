using CRM_App.Data;
using CRM_App.Models;
using System.Collections.ObjectModel;

namespace CRM_App.Views;

[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class CustomerRequestsPage : ContentPage
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
    public ObservableCollection<Request> CustomerRequests { get; set; } = new ObservableCollection<Request>();
    public CustomerRequestsPage()
	{
		InitializeComponent();
        BindingContext = this;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await LoadCustomerRequests();
    }
    private async Task LoadCustomerRequests()
    {
        CustomerName.Text = DatabaseHelper.GetCustomerByIdAsync(_customerId).Result.Name;
        var requests = await DatabaseHelper.GetAllRequestsForCustomerAsync(_customerId);
        CustomerRequests.Clear();
        foreach (var request in requests)
        {
            CustomerRequests.Add(request);
        }
    }
    private async void OnViewRequestClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var requestId = (int)button.CommandParameter;
        await Shell.Current.GoToAsync($"{nameof(UpdateRequestPage)}?RequestId={requestId}");
    }

}