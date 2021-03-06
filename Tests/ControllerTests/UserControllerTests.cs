using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Controllers;
using UserService.Models;
using UserService.Service;
using Xunit;

namespace Tests.ControllerTests
{
    public class UserControllerTests
    {
        private Mock<IUserServices> userService;
        private Mock<ITokenService> tokenService;
        private Mock<ILogger<UserController>> logger;
        private UserController UserController;

        public UserControllerTests()
        {
            userService = new Mock<IUserServices>();
            tokenService = new Mock<ITokenService>();
            logger = new Mock<ILogger<UserController>>();
            UserController = new UserController(userService.Object, logger.Object, tokenService.Object);
        }

        #region GetAllUsersAsync
        [Fact]
        public void VerifyGetAllUsersAsyncTestWhenUserDataIsPresent()
        {
            var data = new List<User>()
            {
                new User{ EmailAddress="abx@de.cs", Password="******", AvatarUrl="imagepath",Name="Test", Username="Test"},
            };
            userService.Setup(x => x.GetAllUsersAsync().Result).Returns(data);

            var res = UserController.GetAllUsersAsync().Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Contains<User>(data[0], (IEnumerable<User>)response.Value);
        }

        [Fact]
        public void VerifyGetAllUsersAsyncTestWhenUserDataIsNotPresent()
        {
            var data = new List<User>();
            userService.Setup(x => x.GetAllUsersAsync().Result).Returns(data);

            var res = UserController.GetAllUsersAsync().Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Empty((IEnumerable<User>)response.Value);
        }

        [Fact]
        public void VerifyGetAllUsersAsyncTestWhenExceptionIsThrown()
        {
            var serviceException = new Exception("A service error occured");
            userService.Setup(x => x.GetAllUsersAsync().Result).Throws(serviceException);

            var res = UserController.GetAllUsersAsync().Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);
        }

        #endregion

        #region SearchUserAsync
        [Fact]
        public void VerifySearchUserAsyncTestForExistingUsername()
        {
            var data = new List<User>()
            {
                new User{ EmailAddress="abx@de.cs", Password="******", AvatarUrl="imagepath",Name="Test", Username="Test"},
            };
            userService.Setup(x => x.SearchUserAsync(It.IsAny<string>()).Result).Returns(data);

            var res = UserController.SearchUserAsync("test").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Contains<User>(data[0], (IEnumerable<User>)response.Value);
        }

        [Fact]
        public void VerifySearchUserAsyncTestForNonExistingUsername()
        {
            var data = new List<User>()
            {

            };
            userService.Setup(x => x.SearchUserAsync(It.IsAny<string>()).Result).Returns(data);

            var res = UserController.SearchUserAsync("test").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Empty((IEnumerable<User>)response.Value);
        }

        [Fact]
        public void VerifySearchUserAsyncTestForBadRequest()
        {
            var argEx = new ArgumentException("Bad username passed");
            userService.Setup(x => x.SearchUserAsync(It.IsAny<string>()).Result).Throws(argEx);

            var res = UserController.SearchUserAsync("test").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 400);

        }

        [Fact]
        public void VerifySearchUserAsyncTestForInternalServerError()
        {
            var ex = new Exception("Internal server error occured");
            userService.Setup(x => x.SearchUserAsync(It.IsAny<string>()).Result).Throws(ex);

            var res = UserController.SearchUserAsync("test").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);

        }
        #endregion

        #region RegisterUserAsync
        [Fact]
        public void VerifyRegisterUserAsyncTestForNonExistingUsername()
        {
            var user = new User { EmailAddress = "abx@de.cs", Password = "******", AvatarUrl = "imagepath", Name = "Test", Username = "Test" };
            userService.Setup(x => x.RegisterUserAsync(It.IsAny<User>()).Result).Returns(user);

            var res = UserController.RegisterUserAsync(user).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 201);
            Assert.Equal(user, (User)response.Value);
        }

        [Fact]
        public void VerifyRegisterUserAsyncTestForExistingUsername()
        {
            var user = new User { EmailAddress = "abx@de.cs", Password = "******", AvatarUrl = "imagepath", Name = "Test", Username = "Test" };
            var argEx = new ArgumentException("User is already registered");
            userService.Setup(x => x.RegisterUserAsync(It.IsAny<User>()).Result).Throws(argEx);

            var res = UserController.RegisterUserAsync(user).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 400);
        }

        [Fact]
        public void VerifyRegisterUserAsyncTestForException()
        {
            var user = new User { EmailAddress = "abx@de.cs", Password = "******", AvatarUrl = "imagepath", Name = "Test", Username = "Test" };
            var ex = new Exception("Internal Server error occured");
            userService.Setup(x => x.RegisterUserAsync(It.IsAny<User>()).Result).Throws(ex);

            var res = UserController.RegisterUserAsync(user).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);
        }
        #endregion

        #region LoginUserAsync

        [Fact]
        public void VerifyLoginUserAsyncTestForValidCredentials()
        {
            var userdata = new UserCredential { Username = "Test", Password = "******" };
            var user = new User { EmailAddress = "abx@de.cs", Password = "******", AvatarUrl = "imagepath", Name = "Test", Username = "Test" };
            userService.Setup(x => x.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()).Result).Returns(user);

            var res = UserController.LoginUserAsync(userdata).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public void VerifyLoginUserAsyncTestForInValidCredentials()
        {
            var userdata = new UserCredential { Username = "Test", Password = "******" };
            var user = new User();
            userService.Setup(x => x.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()).Result).Returns(user);

            var res = UserController.LoginUserAsync(userdata).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 401);
        }

        [Fact]
        public void VerifyLoginUserAsyncTestForBadRequest()
        {
            var userdata = new UserCredential { Username = "Test", Password = "******" };
            var argEx = new ArgumentException("Invalid Arguments passed to login user method");
            userService.Setup(x => x.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()).Result).Throws(argEx);

            var res = UserController.LoginUserAsync(userdata).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 400);
        }

        [Fact]
        public void VerifyLoginUserAsyncTestForException()
        {
            var userdata = new UserCredential { Username = "Test", Password = "******" };
            var ex = new Exception("Internal server error occured");
            userService.Setup(x => x.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()).Result).Throws(ex);

            var res = UserController.LoginUserAsync(userdata).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);
        }
        #endregion

        #region ForgotPasswordAsync

        [Fact]
        public void VerifyForgotPasswordAsyncTestForValidUser()
        {
            var password = new UpdatePassword { Password = "******" };
            var username = "Test";
            userService.Setup(x => x.ForgotPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new OkObjectResult("")));
            var res = UserController.ForgotPasswordAsync(username, password).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public void VerifyForgotPasswordAsyncTestForNonExistingUser()
        {
            var password = new UpdatePassword { Password = "******" };
            var username = "Test";
            userService.Setup(x => x.ForgotPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new ArgumentException("User not found"));
            var res = UserController.ForgotPasswordAsync(username, password).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 400);
        }

        [Fact]
        public void VerifyForgotPasswordAsyncTestForException()
        {
            var password = new UpdatePassword { Password = "******" };
            var username = "Test";
            userService.Setup(x => x.ForgotPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("An error occured"));
            var res = UserController.ForgotPasswordAsync(username, password).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);
        }
        #endregion
    }
}
