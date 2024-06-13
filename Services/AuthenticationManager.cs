using CRM_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
