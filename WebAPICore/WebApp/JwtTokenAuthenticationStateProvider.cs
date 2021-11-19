using App.Repository;
using App.Repository.AuthenticationRepo;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp
{
    public class JwtTokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IAuthenticationRepository authenticationRepository;

        public JwtTokenAuthenticationStateProvider(ITokenRepository tokenRepository,
            IAuthenticationRepository authenticationRepository)
        {
            this.tokenRepository = tokenRepository;
            this.authenticationRepository = authenticationRepository;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var tokenString = await tokenRepository.GetToken();
            if (!string.IsNullOrWhiteSpace(tokenString))
            {
                bool flag = await authenticationRepository.VerifyTokenAsync(tokenString);
                if(flag == false)
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var tokenJwt = tokenHandler.ReadToken(tokenString.Replace("\"", string.Empty)) as JwtSecurityToken;

                if (tokenJwt != null)
                {
                    var claims = new List<Claim>();
                    claims.AddRange(tokenJwt.Claims);

                    var nameIdClaim = tokenJwt.Claims.FirstOrDefault(x => x.Type == "nameid");
                    var nameClaim = tokenJwt.Claims.FirstOrDefault(x => x.Type == "unique_name");
                    var partnerIdClaim = tokenJwt.Claims.FirstOrDefault(x => x.Type == "PartnerId");
                    var roleClaim = tokenJwt.Claims.Where(x => x.Type == "claim").ToList();
                    
                    if (nameIdClaim != null) claims.Add(new Claim(ClaimTypes.Name, nameIdClaim.Value));
                    if (nameClaim != null) claims.Add(new Claim(ClaimTypes.NameIdentifier, nameClaim.Value));
                    if (partnerIdClaim != null) claims.Add(new Claim("PartnerId", partnerIdClaim.Value));

                    foreach (var item in roleClaim)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.Value));
                    }

                    var identity = new ClaimsIdentity(claims, "JWT Token Auth");
                    var principal = new ClaimsPrincipal(identity);

                    return new AuthenticationState(principal);
                }
                else
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
