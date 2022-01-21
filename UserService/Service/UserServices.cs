using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<IEnumerable<User>> SearchUserAsync(string userName)
        {
            return await _userRepository.SearchUserAsync(userName);
        }

        #region Authentication
        public async Task<User> LoginUserAsync(string username, string password)
        {
            return await _userRepository.LoginUserAsync(username, password);
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            return await _userRepository.RegisterUserAsync(user);
        }
        public async Task ForgotPasswordAsync(string userName, string updatedPassword)
        {
           await _userRepository.ForgotPasswordAsync(userName, updatedPassword);
        }
        #endregion
    }
}
