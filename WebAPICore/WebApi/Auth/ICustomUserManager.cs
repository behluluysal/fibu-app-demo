using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Auth
{
    public interface ICustomUserManager
    {
        Task<string> Authenticate(UserLoginViewModel user);
    }
}