using Core.Models;
using Core.ViewModels;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Auth
{
    public class CustomUserManager : ICustomUserManager
    {
        private readonly ICustomTokenManager customTokenManager;
        private readonly AppDbContext _appDbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomUserManager(ICustomTokenManager customTokenManager,
            AppDbContext appDbContext,
           SignInManager<ApplicationUser> signInManager,
           UserManager<ApplicationUser> userManager)
        {
            this.customTokenManager = customTokenManager;
            _appDbContext = appDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<string> Authenticate(UserLoginViewModel user)
        {
            ApplicationUser loggedUser = await _userManager.FindByEmailAsync(user.Email);
            if (loggedUser == null)
            {
                return null;
            }

            //validate credentials !! 3. parameter is for rememberme
            var result = await _signInManager.PasswordSignInAsync(loggedUser.UserName, user.Password,false, lockoutOnFailure: true);
            
            // Log in
            if (result.Succeeded)
            {
                //ApplicationUser loggedUser = await _userManager.FindByEmailAsync(user.Email);
                return await customTokenManager.CreateToken(loggedUser.Id);
            }

            // Account is locked
            if (result.IsLockedOut) return "I am a teapot";

            else return null;
            
        }


    }
}
