using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Service
{
    public interface IUserServices
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
