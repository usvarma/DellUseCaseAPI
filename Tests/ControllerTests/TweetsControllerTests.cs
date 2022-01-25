using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetService.Controllers;
using TweetService.Models;
using TweetService.Service;
using Xunit;

namespace Tests.ControllerTests
{
    public class TweetsControllerTests
    {
        private Mock<ITweetServices> tweetService;
        private Mock<ILogger<TweetsController>> logger;
        private TweetsController TweetController;

        public TweetsControllerTests()
        {
            tweetService = new Mock<ITweetServices>();
            logger = new Mock<ILogger<TweetsController>>();
            TweetController = new TweetsController(tweetService.Object, logger.Object);
        }

        #region GetAllTweetsAsync

        [Fact]
        public void VerifyGetAllTweetsAsyncTestWhenTweetIsPresent()
        {
            var tweetsList = new List<Tweet>()
            {
                new Tweet{ Message = "Test message",Username="Test User",PostedOn = DateTime.Now},
            };
            tweetService.Setup(x => x.GetAllTweetsAsync().Result).Returns(tweetsList);

            var res = TweetController.GetAllTweetsAsync().Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Contains<Tweet>(tweetsList[0], (IEnumerable<Tweet>)response.Value);
        }

        [Fact]
        public void VerifyGetAllTweetsAsyncTestWhenTweetsAreNotPresent()
        {
            var tweetsList = new List<Tweet>();
            tweetService.Setup(x => x.GetAllTweetsAsync().Result).Returns(tweetsList);

            var res = TweetController.GetAllTweetsAsync().Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Empty((IEnumerable<Tweet>)response.Value);
        }

        [Fact]
        public void VerifyGetAllTweetsAsyncTestWhenExceptionIsThrown()
        {
            var tweetsList = new List<Tweet>();
            tweetService.Setup(x => x.GetAllTweetsAsync().Result).Throws(new Exception("An internal server error occured"));

            var res = TweetController.GetAllTweetsAsync().Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);
            
        }
        #endregion

        #region GetAllTweetsOfUserAsync
        [Fact]
        public void VerifyGetAllTweetsOfUserAsyncTestWhenTweetIsPresent()
        {
            var tweetsList = new List<Tweet>()
            {
                new Tweet{ Message = "Test message",Username="Test User",PostedOn = DateTime.Now},
            };
            tweetService.Setup(x => x.GetAllTweetsOfUserAsync(It.IsAny<string>()).Result).Returns(tweetsList);

            var res = TweetController.GetAllTweetsOfUserAsync("Test").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Contains<Tweet>(tweetsList[0], (IEnumerable<Tweet>)response.Value);
        }

        [Fact]
        public void VerifyGetAllTweetsOfUserAsyncTestWhenTweetsAreNotPresent()
        {
            var tweetsList = new List<Tweet>();
            tweetService.Setup(x => x.GetAllTweetsOfUserAsync(It.IsAny<string>()).Result).Returns(tweetsList);

            var res = TweetController.GetAllTweetsOfUserAsync("Test").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 200);
            Assert.Empty((IEnumerable<Tweet>)response.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void VerifyGetAllTweetsOfUserAsyncTestWhenUsernameIsInvalid(string username)
        {
            var res = TweetController.GetAllTweetsOfUserAsync(username).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 400);
        }

        [Fact]
        public void VerifyGetAllTweetsOfUserAsyncTestWhenExceptionIsThrown()
        {
            tweetService.Setup(x => x.GetAllTweetsOfUserAsync(It.IsAny<string>()).Result).Throws(new Exception("An internal server error occured"));

            var res = TweetController.GetAllTweetsOfUserAsync("Test").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);

        }


        #endregion

        #region AddTweetAsync

        [Fact]
        public void VerifyAddTweetAsyncTestWhenTweetIsValid()
        {
            var newtweet = new Tweet { Message = "Test message", Username = "Test User", PostedOn = DateTime.Now };
            tweetService.Setup(x => x.AddTweetAsync(It.IsAny<Tweet>(), It.IsAny<string>()).Result).Returns(newtweet);

            var res = TweetController.AddTweetAsync(newtweet, "Test User").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 201);
            Assert.Equal(newtweet, (Tweet)response.Value);
        }

        [Theory]
        [InlineData(null, "test")]
        
        public void VerifyAddTweetsAsyncTestForNullTweet(Tweet tweet, string username)
        {
            var res = TweetController.AddTweetAsync(tweet, username).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 400);
        }

        [Theory]
        [InlineData("")]
        [InlineData("          ")]
        [InlineData(null)]

        public void VerifyAddTweetsAsyncTestForInvalidUsername(string username)
        {
            var newtweet = new Tweet { Message = "Test message", Username = "Test User", PostedOn = DateTime.Now };
            var res = TweetController.AddTweetAsync(newtweet, username).Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 400);
        }

        [Fact]
        public void VerifyAddTweetAsyncTestWhenExceptionIsThrown()
        {
            var newtweet = new Tweet { Message = "Test message", Username = "Test User", PostedOn = DateTime.Now };
            tweetService.Setup(x => x.AddTweetAsync(It.IsAny<Tweet>(), It.IsAny<string>()).Result).Throws(new Exception("An error occured"));

            var res = TweetController.AddTweetAsync(newtweet, "Test User").Result;
            var response = res as ObjectResult;
            Assert.True(response.StatusCode == 500);
        }
        #endregion
    }
}
