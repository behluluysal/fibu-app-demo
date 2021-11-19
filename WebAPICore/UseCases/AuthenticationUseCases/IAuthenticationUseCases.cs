using Core.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;

namespace UseCases.AuthenticationUseCases
{
    public interface IAuthenticationUseCases
    {
        Task<string> GetUserInfoAsync(string token);
        Task<JsonElement> LoginAsync(UserLoginViewModel user);
        Task Logout();
        Task<bool> VerifyTokenAsync(string token);
    }
}