namespace CRM_App.Views;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
	}

	public void OnLoad(object sender, EventArgs e)
	{
        if (Data.AuthenticationManager.IsLoggedIn)
        {
            Shell.Current.DisplayAlert("Eroare", "Nu suneti logat(a)", "Ok");
            Shell.Current.GoToAsync("///LoginPage");
        }
    }
}