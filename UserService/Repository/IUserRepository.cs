using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> SearchUserAsync(string userName);
        
        #region Authentication
        Task<User> RegisterUserAsync(User user);
        Task<User> LoginUserAsync(string username, string password);
        Task ForgotPasswordAsync(string userName, string updatedPassword);

        #endregion
    }
}
