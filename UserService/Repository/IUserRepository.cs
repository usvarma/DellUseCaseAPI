using System.Collections.Generic;
using UserService.Models;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> SearchUser(string userName);
        
        #region Authentication
        bool RegisterUser(User user);
        bool LoginUser(User user);
        bool ForgotPassword(string userName);

        #endregion
    }
}
