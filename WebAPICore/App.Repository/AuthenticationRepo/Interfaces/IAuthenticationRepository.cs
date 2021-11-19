using Core.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Repository.AuthenticationRepo
{
    public interface IAuthenticationRepository
    {
        Task<string> GetUserInfoAsync(string token);
        Task<JsonElement> LoginAsync(UserLoginViewModel user);
        Task<bool> VerifyTokenAsync(string token);
    }
}