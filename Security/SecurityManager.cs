using ProjetLaVictoireDesServices   .Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetLaVictoireDesServices.Security
{
    public class SecurityManager
    {
        public async void SignIn(HttpContext httpContext, Compte compte)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(getUserClaims(compte),
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
        private IEnumerable<Claim> getUserClaims(Compte compte)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, compte.NomUtilisateur));
            claims.Add(new Claim(ClaimTypes.Role, compte.Role.Nom));
            return claims;
        }

    }
}
