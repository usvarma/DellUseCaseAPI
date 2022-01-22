using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Service
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(string userName, string emailId, string name, string image);
        Task<User> GetUserInfoFromTokenAsync(string token);
        Task<bool> VerifyTokenAsync(string token);
        Task<string> RefreshTokenAsync(string tokenString);
    }
}
