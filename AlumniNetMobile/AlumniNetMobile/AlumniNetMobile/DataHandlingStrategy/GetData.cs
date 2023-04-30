using AlumniNetMobile.Common;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.DataHandlingStrategy
{
    public class GetData : ManageDataStrategy
    {
        public override async Task<string> ManageData(HttpClient httpClient, string endPoint, string json)
        {
            var uri = new Uri(Uri + endPoint);
            try
            {
                var response = await httpClient.GetAsync(uri);

                if (!response.IsSuccessStatusCode) return string.Empty;

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Stream> ManageStreamData(string endPoint,string token="")
        {
            HttpClient httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            if (token.Length > 0)
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uri = new Uri(Uri + endPoint);
            try
            {
                var response = await httpClient.GetAsync(uri);

                if (!response.IsSuccessStatusCode) return Stream.Null;

                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
