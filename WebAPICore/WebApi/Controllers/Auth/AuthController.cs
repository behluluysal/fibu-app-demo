using AutoWrapper.Wrappers;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Auth;

namespace WebAPI.Controllers.Auth
{
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly ICustomUserManager _customUserManager;
        private readonly ICustomTokenManager _customTokenManager;

        public AuthController(ICustomUserManager customUserManager,
            ICustomTokenManager customTokenManager)
        {
            _customUserManager = customUserManager;
            _customTokenManager = customTokenManager;
        }

        //return type is string, should be changed to json
        [HttpPost]
        [Route("/authenticate")]
        public async Task<ApiResponse> Authenticate([FromBody] UserLoginViewModel userCredential)
        {
            string token = await _customUserManager.Authenticate(userCredential);
            //Invalid login attempt
            if (string.IsNullOrEmpty(token))
            {
                throw new ApiProblemDetailsException("Invalid login attempt",401);
            }
            if(token == "I am a teapot")
            {
                throw new ApiProblemDetailsException("I am a teapot", 418);
            }
            else
                return new ApiResponse(token, 200);
        }

        [HttpGet]
        [Route("/verifytoken/{token}")]
        public Task<bool> Verify(string token)
        {
            return Task.FromResult(_customTokenManager.VerifyToken(token));
        }

        [HttpGet]
        [Route("/getuserinfo")]
        public Task<string> GetUserInfoByToken(string token)
        {
            return Task.FromResult(_customTokenManager.GetUserInfoByToken(token));
        }
    }
}
