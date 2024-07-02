using CRM_App.Data;
using CRM_App.ViewModels;
using CRM_App.Views;

namespace CRM_App.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		if (PasswordEntry != null)
		{
			PasswordEntry.Text = string.Empty;
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //stergem campul parola cand apare
        PasswordEntry.Text = string.Empty;
    }

}