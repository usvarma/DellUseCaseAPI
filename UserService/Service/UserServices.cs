using System;
using System.Collections.Generic;
using UserService.Models;
using UserService.Repository;

namespace UserService.Service
{
    public class UserServices: IUserServices
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public IEnumerable<User> SearchUser(string userName)
        {
            return _userRepository.SearchUser(userName);
        }

        #region Authentication
        public bool LoginUser(User user)
        {
            var loginResult = _userRepository.LoginUser(user);
            return loginResult;
        }

        public bool RegisterUser(User user)
        {
            var registerResult = _userRepository.RegisterUser(user);
            return registerResult;
        }
        public bool ForgotPassword(string userName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
