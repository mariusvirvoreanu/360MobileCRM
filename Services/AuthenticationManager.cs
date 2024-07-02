using CRM_App.Models;

namespace CRM_App.Data
{
    public static class AuthenticationManager
    {
        private static bool isLoggedIn = false;
        public static bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set { isLoggedIn = value; }
        }
        public static User LoggedUser { get; set; }
    }
}
