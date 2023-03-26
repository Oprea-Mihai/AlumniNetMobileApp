using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.Common
{
    public interface IAuthenticationService
    {
        bool IsSignedIn();
    }
}
