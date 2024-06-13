using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CRM_App.Models;
using System.ComponentModel;
using CRM_App.Services;
using CRM_App.Data;
using BCrypt.Net;

namespace CRM_App.ViewModels
{
    public partial class RegisterViewModel : ObservableObject, INotifyPropertyChanged
    {
        private string username;
        private string password;
        private string confirmPassword;

        public string Username
        {
            get => username;
            set
            {
                SetProperty(ref username, value);
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }
        public bool IsRegisterEnabled => !string.IsNullOrWhiteSpace(Username) 
            && !string.IsNullOrWhiteSpace(Password) 
            && !string.IsNullOrWhiteSpace(ConfirmPassword) 
            && Password == ConfirmPassword 
            && Username.Length>5 
            && ConfirmPassword.Length>7;

        public RegisterViewModel()
        {
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
            GoToLoginCommand = new AsyncRelayCommand(GoToLogin);
        }

        public IAsyncRelayCommand RegisterCommand { get; }
        public IAsyncRelayCommand GoToLoginCommand { get; }

        private async Task RegisterAsync()
        {
            //Check if the username already exists
            var existingUser = await DatabaseHelper.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare creare cont", "Contul exista deja", "OK");
                return;
            }
            //Check if passwords match
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare creare cont", "Eroare verificare parola", "OK");
                return;
            }
            //Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
            // Create and add the new user
            var newUser = new User 
            { 
                Username = Username, 
                Password = hashedPassword, 
                Role = "agent" 
            };
            int result = await DatabaseHelper.AddUserAsync(newUser);
            if (result == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare creare cont", "Contul nu a putut fi creat", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Succes înregistrare cont", "Pute-ți să vă logați", "OK");
                await Shell.Current.GoToAsync("///LoginPage");
            }
        }
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }

        //private async Task RegisterAsync()
        //{
        //    // Check if the username already exists
        //    var existingUser = App.DatabaseHelper.GetUserByUsername(Username);
        //    if (existingUser != null)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Registration Failed", "Username already exists", "OK");
        //        return;
        //    }
        //    // Check if passwords match
        //    if (Password != ConfirmPassword)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Registration Failed", "Passwords do not match", "OK");
        //        return;
        //    }
        //    // Hash the password
        //    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
        //    // Create and add the new user
        //    var newUser = new User { Username = Username, Password = hashedPassword, Role = "agent" };
        //    App.DatabaseHelper.AddUser(newUser);

        //    await Application.Current.MainPage.DisplayAlert("Registration Successful", "You can now log in", "OK");
        //    await Shell.Current.GoToAsync("///LoginPage");
        //}
    }
}
