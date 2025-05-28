using CoreLayer.Entities.Identity;
using CoreLayer.Serveses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class TokenServices : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser User, UserManager<ApplicationUser> userManager)
        {

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, User.Id),
            new Claim(ClaimTypes.Email, User.Email),
            new Claim(ClaimTypes.GivenName, User.FullName ?? ""),
            new Claim(JwtRegisteredClaimNames.Sub, User.Id),
            


        };

            var userRoles = await userManager.GetRolesAsync(User);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
