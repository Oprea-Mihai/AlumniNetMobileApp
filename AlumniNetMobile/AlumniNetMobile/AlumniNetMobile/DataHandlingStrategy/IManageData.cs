using System.Threading.Tasks;

namespace AlumniNetMobile.DataHandlingStrategy
{
    public interface IManageData
    {
        Task<T> GetDataAndDeserializeIt<T>(string url, string json = "", string token = "");
        void SetStrategy(ManageDataStrategy manageDataStrategy);
    }
}