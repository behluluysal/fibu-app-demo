using System.Text.Json;
using System.Threading.Tasks;

namespace App.Repository.ApiClient
{
    public interface IWebApiExecuter
    {
        Task<T> InvokePost<T>(string uri, T obj);
        Task<T> InvokeGet<T>(string uri);
        Task InvokePut<T>(string uri, T obj);
        Task InvokeDelete(string uri);
        Task<JsonElement> InvokePostReturnsString<T>(string uri, T obj);
    }
}