using System.Windows.Input;

namespace CRM_App.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set
            {
                if (_welcomeMessage != value)
                {
                    _welcomeMessage = value;
                    OnPropertyChanged(nameof(WelcomeMessage));
                }
            }
        }
        public ICommand LogoutCommand { get; }

        public MainPageViewModel()
        {
            WelcomeMessage = "Bine ati venit in pagina principala!";
            LogoutCommand = new Command(MainPageViewModel.OnLogout);
        }

        public static void OnLogout()
        {
            Shell.Current.GoToAsync("///LoginPage").Wait();
            Application.Current.MainPage.DisplayAlert("Logout", "Ati fost deconectat(a)", "Ok");
        }

    }
}
