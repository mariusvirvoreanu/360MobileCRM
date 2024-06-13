using CRM_App.Data;

namespace CRM_App.Views;

public partial class UserRequestsPage : ContentPage
{
	public UserRequestsPage()
	{
		InitializeComponent();
        Username.Text = AuthenticationManager.LoggedUser.Username;
        LoadRequests();
    }

    private async void LoadRequests()
    {
        var requests = await DatabaseHelper.GetAllRequestsForUserAsync(AuthenticationManager.LoggedUser.UserID);
        RequestsCollectionView.ItemsSource = requests;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadRequests();
    }
    private async void OnViewRequestClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var requestId = (int)button.CommandParameter;
        await Shell.Current.GoToAsync($"{nameof(UpdateRequestPage)}?RequestId={requestId}");
    }
}