using AlumniNetMobile.Common;
using AlumniNetMobile.Droid.Services;
using System;
using System.Net.Http;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpClientHandlerService))]
namespace AlumniNetMobile.Droid.Services
{
    public class HttpClientHandlerService : IHttpClientHandlerService
    {
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

    }
}