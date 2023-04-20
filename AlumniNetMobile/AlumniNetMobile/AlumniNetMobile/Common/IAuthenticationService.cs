using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlumniNetMobile.Common
{
    public interface IAuthenticationService
    {
        bool IsSignedIn();
        Task<bool> CreateUser(string username, string email, string password);
        void SignOut();
        Task<string> SignIn(string email, string password);
        Task ResetPassword(string email);
        Task<string> GetCurrentTokenAsync();
    }
}
