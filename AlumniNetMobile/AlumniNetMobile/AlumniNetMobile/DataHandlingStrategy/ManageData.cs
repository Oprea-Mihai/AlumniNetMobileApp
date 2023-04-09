using AlumniNetMobile.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.DataHandlingStrategy
{
    public class ManageData
    {
        private ManageDataStrategy _manageDataStrategy;
        private HttpClient _httpClient;

        public void SetStrategy(ManageDataStrategy manageDataStrategy)
        {
            _manageDataStrategy = manageDataStrategy;
        }

        public async Task<T> GetDataAndDeserializeIt<T>(string url, string json)
        {
            _httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var data = await _manageDataStrategy.ManageData(_httpClient, url, json);

            if (data == string.Empty)
                throw new TaskCanceledException();

            var deserializedData = JsonConvert.DeserializeObject<T>(data);
            return deserializedData;
        }
    }
}
