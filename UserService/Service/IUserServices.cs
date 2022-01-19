using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Service
{
    public interface IUserServices
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
