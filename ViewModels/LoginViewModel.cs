using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CRM_App.Data;

namespace CRM_App.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private string username;
        private string password;

        public string Username
        {
            get => username;
            set
            {
                SetProperty(ref username, value);
                OnPropertyChanged(nameof(IsLoginEnabled));
            }
        }
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                OnPropertyChanged(nameof(IsLoginEnabled));
            }
        }
        public bool IsLoginEnabled => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync, () => IsLoginEnabled);
            GoToRegisterCommand = new AsyncRelayCommand(GoToRegisterAsync);
        }

        public IAsyncRelayCommand LoginCommand { get; }
        public IAsyncRelayCommand GoToRegisterCommand { get; }

        private async Task LoginAsync()
        {
            var user = await DatabaseHelper.GetUserByUsernameAsync(Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Logare cu succes", "Bine ai venit!", "OK");
                AuthenticationManager.IsLoggedIn = true;
                AuthenticationManager.LoggedUser = user;
                //Navigare la pagina principala
                await Shell.Current.GoToAsync("///MainPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Logare esuata", "Credentiale invalide!", "OK");
            }
        }
        private async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync("///RegisterPage");
        }

    }
}
