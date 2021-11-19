using App.Repository;
using App.Repository.AuthenticationRepo;
using Core.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;

namespace UseCases.AuthenticationUseCases
{
    public class AuthenticationUseCases : IAuthenticationUseCases
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ITokenRepository _tokenRepository;

        public AuthenticationUseCases(IAuthenticationRepository authenticationRepository,
            ITokenRepository tokenRepository)
        {
            this._authenticationRepository = authenticationRepository;
            this._tokenRepository = tokenRepository;
        }

        public async Task<JsonElement> LoginAsync(UserLoginViewModel user)
        {
            return await _authenticationRepository.LoginAsync(user);
        }

        public async Task<string> GetUserInfoAsync(string token)
        {
            return await _authenticationRepository.GetUserInfoAsync(token);
        }

        public async Task<bool> VerifyTokenAsync(string token)
        {
            return await _authenticationRepository.VerifyTokenAsync(token);
        }


        public async Task Logout()
        {
            await _tokenRepository.SetToken(string.Empty);
        }
    }
}
