using AlumniNetMobile.Common;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.DataHandlingStrategy
{
    public class UpdateData : ManageDataStrategy
    {
        public async override Task<string> ManageData(HttpClient httpClient, string endPoint, string json)
        {
            var uri = new Uri(Uri + endPoint);
            try
            {
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(uri, stringContent);

                if (!response.IsSuccessStatusCode) return string.Empty;

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<string> ManageStreamData(string endPoint, Stream fileStream, string token = "",string filename="img")
        {
            HttpClient httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());

            var uri = new Uri(Uri + endPoint);
            try
            {
                var content = new MultipartFormDataContent
                {
                    { new StreamContent(fileStream), "file",filename}//check how to change this
                };

                if (token.Length > 0)
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PutAsync(uri, content);


                if (!response.IsSuccessStatusCode) 
                    return string.Empty;

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
