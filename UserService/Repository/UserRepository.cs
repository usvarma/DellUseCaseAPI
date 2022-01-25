﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
using UserService.UserDBContext;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly UserDbContext _userDbcontext;
        public UserRepository(UserDbContext userDbcontext)
        {
            _userDbcontext = userDbcontext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userDbcontext.Users.Find(user => user.EmailAddress != null).ToListAsync();
        }
        public async Task<IEnumerable<User>> SearchUserAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException($"Username to search should not be null or empty. Provided username is: {userName}");
            }

            return await _userDbcontext.Users.Find(user => user.Username != null && user.Username.Contains(userName)).ToListAsync();
        }

        #region Authentication
        public async Task<User> LoginUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"Invalid username or password. Username: {username}, Password: {password}");
            }
            
            var user = await SearchUserAsync(username);
                        
            if (!user.Any())
            {
                throw new ArgumentException($"No user with username {username} found");
            }

            return VerifyPassword(user.FirstOrDefault(), password) ? user.FirstOrDefault() : new User();
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            if(user == null)
            {
                throw new ArgumentException($"Invalid argument to register user: {user}");
            }

            var isUserAlreadyRegistered = GetUserList(user.Username).Any();
            if (isUserAlreadyRegistered)
            {
                throw new ArgumentException($"User is already registered with username: {user.Username}");
            }

            var userList = _userDbcontext.Users.AsQueryable().ToList();
            if (userList.Any())
            {
                user.UserId = userList.Max(u => u.UserId) + 1;
            }
            else
            {
                user.UserId = 1;
            }
            
            user.CreatedOn = DateTime.Now;
            user.PasswordHashed = ComputeHash(user.Password);
            await _userDbcontext.Users.InsertOneAsync(user);
            return user;
        }
        public async Task ForgotPasswordAsync(string userName, string updatedPassword)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(updatedPassword))
            {
                throw new ArgumentException($"Invalid arguments to update password. Username: {userName}, password to update:{updatedPassword}");
            }

            var userList = GetUserList(userName);

            if (!userList.Any())
            {
                throw new ArgumentException($"Cannot find username: {userName}");
            }

            userList.FirstOrDefault().PasswordHashed = ComputeHash(updatedPassword);
            await _userDbcontext.Users.ReplaceOneAsync(user => user.Username == userName, userList.FirstOrDefault());
        }

        #endregion

        #region Private Methods
        private IEnumerable<User> GetUserList(string userName)
        {
            return _userDbcontext.Users.Find(user => user.Username == userName).ToList();
        }
        private static HashedPassword ComputeHash(string password)
        {
            var hashAlgo = SHA512.Create();
            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = GenerateSalt(64);
            var passwordWithSalt = new List<byte>();
            passwordWithSalt.AddRange(passwordBytes);
            passwordWithSalt.AddRange(saltBytes);

            var hashedPassword = hashAlgo.ComputeHash(passwordWithSalt.ToArray());

            return new HashedPassword(Convert.ToBase64String(hashedPassword), Convert.ToBase64String(saltBytes));
        }
        private static byte[] GenerateSalt(int keysize)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[keysize];
            rng.GetNonZeroBytes(saltBytes);
            return saltBytes;
        }
        private static bool VerifyPassword(User user, string password)
        {
            var hashAlgo = SHA512.Create();

            if (string.IsNullOrWhiteSpace(password) || user == null)
            {
                return false;
            }

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = Convert.FromBase64String(user.PasswordHashed.SaltUnhashed);
            var passwordWithSalt = new List<byte>();
            passwordWithSalt.AddRange(passwordBytes);
            passwordWithSalt.AddRange(saltBytes);

            var hashedPassword = Convert.ToBase64String(hashAlgo.ComputeHash(passwordWithSalt.ToArray()));

            return hashedPassword == user.PasswordHashed.PasswordHashed;
        }

        #endregion
    }
}
