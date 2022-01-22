using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Service
{
    public class TokenService : ITokenService
    {
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly byte[] secretKey;

        public TokenService(IConfiguration configuration)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtSecretKey"));
        }
        public Task<string> CreateTokenAsync(string userName, string emailId, string name, string image)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                   new Claim[]
                   {
                       new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                       new Claim(JwtRegisteredClaimNames.Email, emailId),
                       new Claim(JwtRegisteredClaimNames.Name, name),
                       new Claim(JwtRegisteredClaimNames.GivenName,image) //Added Image url as GivenName
                   }
                 ),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public Task<User> GetUserInfoFromTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            var jwtToken = tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken;
            User user = new User();
            bool userDataFound = false;
            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName);
            if (claim != null)
            {
                userDataFound = true;
                user.Username = claim.Value;
            }
            claim = null;
            claim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email);
            if (claim != null)
            {
                userDataFound = true;
                user.EmailAddress = claim.Value;
            }
            claim = null;
            claim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name);
            if (claim != null)
            {
                userDataFound = true;
                user.Name = claim.Value;
            }
            claim = null;
            claim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.GivenName);
            if (claim != null)
            {
                userDataFound = true;
                user.AvatarUrl = claim.Value;
            }
            if (userDataFound) return Task.FromResult(user);
            return null;
        }

        public async Task<string> RefreshTokenAsync(string tokenString)
        {
            User user = await GetUserInfoFromTokenAsync(tokenString);
            if (user == null) return null;
            return await CreateTokenAsync(user.Username, user.EmailAddress, user.Name, user.AvatarUrl);
        }

        public Task<bool> VerifyTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return Task.FromResult(false);

            SecurityToken securityToken;

            try
            {
                tokenHandler.ValidateToken(
                token.Replace("\"", string.Empty),
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
                return Task.FromResult(false);
            }
            catch (Exception)
            {
                throw;
            }
            return Task.FromResult(securityToken != null);
        }
    }
}