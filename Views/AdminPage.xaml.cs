using CRM_App.Data;
using CRM_App.Models;

namespace CRM_App.Views;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }
    private async Task LoadData()
    {
        var users = await DatabaseHelper.GetAllUsersAsync();
        
        UsersListView.ItemsSource = users.Where(u=>u.Role !="admin");

        var products = await DatabaseHelper.GetAllProductsAsync();
        ProductsListView.ItemsSource = products;
    }
    private async void OnUserSelected(object sender, ItemTappedEventArgs e)
    {
        var user = e.Item as User;
        if (user == null)
            return;

        bool resetPassword = await DisplayAlert("Resetare parola", $"Resetam parola pentru: {user.Username} ?", "Da", "Nu");
        if (resetPassword)
        {
            string newPassword = await DisplayPromptAsync("Parola noua", "Introduceti noua parola:","Ok","Renunta");
            if (newPassword == null)
            {
                return;
            }
            if (newPassword.Length < 6)
            {
                await DisplayAlert("Eroare", "Parola trebuie sa aiba minim 6 caractere!", "Ok");
                return;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword).Trim();
            int result = await DatabaseHelper.UpdateUserPasswordAsync(user.UserID, hashedPassword);
            if (result == 0)
            {
                await DisplayAlert("Eroare", "Nu am putut reseta parola", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Parola resetata cu succes", "Ok");
            }
            return;
        }

        bool deleteUser = await DisplayAlert("Stergere utilizator", $"Stergem utilizator: {user.Username} ?", "Da", "Nu");
        if (deleteUser)
        {
            int result = await DatabaseHelper.DeleteUserAsync(user.UserID);
            if (result == 0)
            {
                await DisplayAlert("Eroare", "Nu am putut sterge utilizatorul", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Utilizator sters", "Ok");
            }
            await LoadData();
        }
    }
    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddProductPage));
    }
    private async void OnProductSelected(object sender, ItemTappedEventArgs e)
    {
        var product = e.Item as Product;
        if (product == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(UpdateProductPage)}?ProductId={product.ProductID}");
    }
}