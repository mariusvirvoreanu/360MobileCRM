using CommunityToolkit.Mvvm.Input;
using CRM_App.Data;
using System.Windows.Input;
using BCrypt.Net;

namespace CRM_App.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {
        private string username;
        private string role;
        private string newPassword;
        private string confirmPassword;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }
        public string Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }
        public string NewPassword
        {
            get => newPassword;
            set
            {
                SetProperty(ref newPassword, value);
                OnPropertyChanged(nameof(IsUpdateEnabled));
            }
        }
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
                OnPropertyChanged(nameof(IsUpdateEnabled));
            }
        }
        public bool IsUpdateEnabled => !string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword;

        public ICommand UpdatePasswordCommand { get; }
        public ICommand DeleteAccountCommand { get; }
        public ICommand GoToMainCommand { get; }

        public UserViewModel()
        {
            UpdatePasswordCommand = new Command(UpdatePassword);
            DeleteAccountCommand = new Command(DeleteAccount);
            GoToMainCommand = new Command(GoToMainPage);

            // Load user details
            LoadUserDetails();
        }

        private void LoadUserDetails()
        {
            Username = Data.AuthenticationManager.LoggedUser.Username;
            Role = Data.AuthenticationManager.LoggedUser.Role;
        }
        private async void GoToMainPage()
        {
            await Shell.Current.GoToAsync("///MainPage");
        }
        private async void UpdatePassword()
        {
            if (!string.IsNullOrEmpty(NewPassword) && NewPassword.Length > 5)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                int result = await DatabaseHelper.UpdateUserPasswordAsync(AuthenticationManager.LoggedUser.UserID, hashedPassword);
                if (result == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Erroare", "Nu am putut actualiza parola", "OK");
                }
                else if (result == -1)
                {
                    await Application.Current.MainPage.DisplayAlert("Erroare", "Ati introdus aceiasi parola", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Info", "Parola actualizata cu succes", "OK");
                    await Shell.Current.GoToAsync("///LoginPage");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Noua parola trebuie sa aiba minim 6 caractere", "OK");
            }
        }
        private async void DeleteAccount()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete your account?", "Yes", "No");
            if (confirm)
            {
                int result = await DatabaseHelper.DeleteUserAsync(AuthenticationManager.LoggedUser.UserID);
                if (result == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Deleted", "Contul a fost sters", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Eroare", "Nu am putut sterge contul", "OK");
                }
                // Navigate to the login page after deletion
                await Shell.Current.GoToAsync("///LoginPage");
            }
        }



        //private async void UpdatePassword()
        //{
        //    //if ( !string.IsNullOrEmpty(NewPassword) && NewPassword.Length>5) 
        //    //{
        //    //    int result = App.DatabaseHelper.UpdateUser(AuthenticationManager.LoggedUser, ConfirmPassword);
        //    //    if (result == 0)
        //    //    {
        //    //        await Application.Current.MainPage.DisplayAlert("Erroare", "Nu am putut actualiza parola", "OK");
        //    //    }
        //    //    else if (result == -1)
        //    //    {
        //    //        await Application.Current.MainPage.DisplayAlert("Erroare", "Ati introdus aceiasi parola", "OK");
        //    //    }
        //    //    else
        //    //    {
        //    //        await Application.Current.MainPage.DisplayAlert("Info", "Parola actualizata cu succes", "OK");
        //    //        await Shell.Current.GoToAsync("///LoginPage");
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    await Application.Current.MainPage.DisplayAlert("Eroare", "Noua parola trebuie sa aiba minim 6 caractere", "OK");
        //    //}
        //}
        //private async void DeleteAccount()
        //{
        //    //// Implement account deletion logic
        //    //bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete your account?", "Yes", "No");
        //    //if (confirm)
        //    //{
        //    //    // Delete account logic
        //    //    int result = App.DatabaseHelper.DeleteUser(AuthenticationManager.LoggedUser.UserID);
        //    //    if (result == 0)
        //    //    {
        //    //        await Application.Current.MainPage.DisplayAlert("Deleted", "Contul a fost sters", "OK");
        //    //    }
        //    //    else
        //    //    {
        //    //        await Application.Current.MainPage.DisplayAlert("Eroare", "Nu am putut sterge contul", "OK");
        //    //    }
        //    //    // Navigate to the login page after deletion
        //    //    await Shell.Current.GoToAsync("///LoginPage");
        //    //}
        //}
    }
}
