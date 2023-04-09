using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AlumniNetMobile.Common
{

    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
