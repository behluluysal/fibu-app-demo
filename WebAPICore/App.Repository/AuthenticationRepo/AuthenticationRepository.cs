using App.Repository.ApiClient;
using Core.ViewModels;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Repository.AuthenticationRepo
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IWebApiExecuter webApiExecuter;
        private readonly ITokenRepository tokenRepository;

        

        public AuthenticationRepository(IWebApiExecuter webApiExecuter, ITokenRepository tokenRepository)
        {
            this.webApiExecuter = webApiExecuter;
            this.tokenRepository = tokenRepository;
        }

        public async Task<JsonElement> LoginAsync(UserLoginViewModel user)
        {
            var token = await webApiExecuter.InvokePostReturnsString("authenticate",user);
            var deserializeToken = token.GetProperty("result").ToString();
            await tokenRepository.SetToken((string)(deserializeToken));
            return token;
        }
        

        public async Task<bool> VerifyTokenAsync(string token)
        {
            var response = await webApiExecuter.InvokeGet<dynamic>($"verifytoken/{token}");
            if (response.GetProperty("result").ToString() == "True")
            {
                return true;
            }   
            else
            {
                return false;
            }
                
        }
        public async Task<string> GetUserInfoAsync(string token)
        {
            Console.WriteLine(token);
            /*
            var userName = await this.webApiExecuter.InvokePostReturnsString("getuserinfo", new { token = token });
            if (string.IsNullOrWhiteSpace(userName) || userName == "\"\"") return null;
            */
            return "";
        }
    }
}
