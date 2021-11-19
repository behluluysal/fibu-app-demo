using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Auth
{
    public interface ICustomTokenManager
    {
        Task<string> CreateToken(string userId);
        string GetUserInfoByToken(string token);
        bool VerifyToken(string token);
    }
}