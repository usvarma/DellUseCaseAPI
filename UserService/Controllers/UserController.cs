﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Service;

namespace UserService.Controllers
{
    [Route("")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServices _userService;
        public UserController(IUserServices userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        /// <summary>
        /// Get all registered users
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("/api/v1.0/tweets/users/all")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var userList = await _userService.GetAllUsersAsync();
                return Ok(userList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while searching for users. Exception is {ex}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Search for an user using username. Can search for full or partial match(Wildcard search).
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        // Search for an username
        [HttpGet]
        [Route("/api/v1.0/tweets/users/search/{username}/")]
        public IActionResult SearchUser([FromRoute] string username)
        {
            try
            {
                var userList = _userService.SearchUserAsync(username);
                return Ok(userList);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError($"Could not search for an user due to exception. Exception is {argEx}");
                return StatusCode(400, $"{argEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while searching for user. Exception is {ex}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        #region Authentication

        /// <summary>
        /// Register an user
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1.0/tweets/register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] User user)
        {
            try
            {
                var registeredUser = await _userService.RegisterUserAsync(user);
                if (registeredUser != null)
                {
                    return Created("", registeredUser);
                }
                else
                {
                    return BadRequest("Username already exists");
                }
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError($"Could not register user due to exception. Exception is {argEx}");
                return StatusCode(400, $"{argEx.Message}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Internal server error occured while registering user. Exception is {ex}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1.0/tweets/login")]
        public async Task<IActionResult> LoginUser([FromBody] UserCredential userdata)
        {
            
            return Ok();
        }

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/forgot")]
        public async Task<IActionResult> ForgotPasswordAsync([FromRoute]string username, [FromBody] UpdatePassword password)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(username) || password == null)
                {
                    return BadRequest($"Username or password should not be null");
                }
                await _userService.ForgotPasswordAsync(username, password.Password);
                return await Task.FromResult(Ok($"Successfully changed password for the user {username}"));
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError($"Could not update password due to exception. Exception is {argEx}");
                return StatusCode(400, $"{argEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while updating password. Exception is {ex}");
                return StatusCode(500, $"{ex.Message}");
            }
        }
        #endregion
    }
}