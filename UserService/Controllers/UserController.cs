using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Service;

namespace UserService.Controllers
{
    [Route("")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserServices _userService;
        public UserController(IUserServices userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Get all registered users
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("/api/v1.0/tweets/users/all")]
        public IActionResult GetAllUsers()
        {
            var userList = _userService.GetAllUsers();
            return Ok(userList);
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
            var userList = _userService.SearchUser(username);
            return Ok(userList);
        }

        #region Authentication

        /// <summary>
        /// Register an user
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1.0/tweets/register")]
        public IActionResult RegisterUser([FromBody] User user)
        {
            var result = _userService.RegisterUser(user);
            if (result)
            {
                return Created("", result);
            }
            else
            {
                return BadRequest("Username already exists");
            }
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1.0/tweets/login")]
        public IActionResult LoginUser([FromBody] UserCredential userdata)
        {
            return Ok();
        }

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("/api/v1.0/tweets/{username}/forgot")]
        public IActionResult ForgotPassword(string username, [FromBody] UpdatePassword password)
        {
            return Ok();
        }
        #endregion
    }
}
