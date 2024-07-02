using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CRM_App.Data;
using CRM_App.Models;
using System.ComponentModel;

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
            //Verificam daca username exista
            var existingUser = await DatabaseHelper.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare creare cont", "Contul exista deja", "OK");
                return;
            }
            //Verificam parola
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Eroare creare cont", "Eroare verificare parola", "OK");
                return;
            }
            //Hash parola
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
            //Creare utilizator nou
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
                await Application.Current.MainPage.DisplayAlert("Succes înregistrare cont", "Puteți să vă logați", "OK");
                await Shell.Current.GoToAsync("///LoginPage");
            }
        }
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }

    }
}
