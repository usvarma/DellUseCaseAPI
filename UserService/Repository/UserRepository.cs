using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
using UserService.UserDBContext;

namespace UserService.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly UserDbContext _userDbcontext;
        public UserRepository(UserDbContext userDbcontext)
        {
            _userDbcontext = userDbcontext;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userDbcontext.Users.Find(user => user.Username != null).ToList();
        }
        public IEnumerable<User> SearchUser(string userName)
        {
            return _userDbcontext.Users.Find(user => user.Username != null && user.Username.Contains(userName)).ToList();
        }

        #region Authentication
        public bool LoginUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool RegisterUser(User user)
        {
            var userList = _userDbcontext.Users.AsQueryable().ToList();
            if (userList.Any(u => u.Username == user.Username))
            {
                return false;
            }

            if (userList.Count() > 0)
            {
                user.UserId = userList.Max(u => u.UserId) + 1;
            }
            else
            {
                user.UserId = 1;
            }
            user.CreatedOn = DateTime.Now;
            _userDbcontext.Users.InsertOne(user);
            return true;
        }
        public bool ForgotPassword(string userName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
