using Core.Models;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Auth
{
    public class JwtTokenManager : ICustomTokenManager
    {
        private JwtSecurityTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _db;
        private byte[] secretKey;

        public JwtTokenManager(IConfiguration configuration, 
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager,
            AppDbContext db)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSecretKey"));
        }
        public async Task<string> CreateToken(string userId)
        {
            List<Claim> claims = new List<Claim>();
            

            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            //get user role names
            IList<string> roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            if (roles.Contains("BusinessPartner"))
            {
                BusinessPartner businessPartner = _db.BusinessPartners.FirstOrDefault(x => x.Email == user.Email);
                if(businessPartner != null)
                {
                    claims.Add(new Claim("PartnerId", businessPartner.Id.ToString()));
                }
                else
                {
                    BPResponsiblePerson bPResponsiblePerson = _db.BPResponsiblePeople.FirstOrDefault(x => x.Emails.Any(y => y.Email == user.Email));
                    if (bPResponsiblePerson != null)
                    {
                        claims.Add(new Claim("PartnerId", bPResponsiblePerson.BusinessPartnerId.ToString()));
                    }
                    else
                    {
                        // Probably System Error
                        return null;
                    }
                }
            }

            //create an empty list for userClaims
            IEnumerable<Claim> userClaims = Enumerable.Empty<Claim>();

            //fill userClaim with associated claims 
            foreach (var item in roles)
            {
                userClaims = userClaims.Concat(await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(item)));
            }

            foreach (var item in userClaims)
            {
                claims.Add(new Claim("claim", item.Value));
            }


            //we'll define payload(subject,expires), header is signingCredentials, signature will be generated and will be combined with payload and header then become a jwt token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256Signature
                    )
            };
            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

       
        public string GetUserInfoByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var jwtToken = _tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken;
            var claim = jwtToken.Claims.FirstOrDefault(x=>x.Type =="nameid");
            if (claim != null) return claim.Value;
            return null;
        }

        public bool VerifyToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            SecurityToken securityToken;

            try
            {
                _tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out securityToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            return securityToken != null;
        }
    }
}
