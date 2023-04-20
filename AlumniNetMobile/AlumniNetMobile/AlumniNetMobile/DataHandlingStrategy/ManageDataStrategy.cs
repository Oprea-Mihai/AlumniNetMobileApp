using System.Net.Http;
using System.Threading.Tasks;

namespace AlumniNetMobile.DataHandlingStrategy
{
    public abstract class ManageDataStrategy
    {        
        protected const string Uri = "https://2fg0l4p5-7290.euw.devtunnels.ms/api/";
        //protected const string Uri = "https://10.0.2.2:7290/api/";
        public abstract Task<string> ManageData(HttpClient httpClient, string endPoint, string json);
    }
}
