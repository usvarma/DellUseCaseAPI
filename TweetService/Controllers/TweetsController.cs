using Microsoft.AspNetCore.Mvc;
using TweetService.Models;
using TweetService.Service;

namespace TweetService.Controllers
{
    [Route("")]
    [ApiController]
    public class TweetsController : ControllerBase
    {

        private readonly ITweetServices _tweetService;
        public TweetsController(ITweetServices tweetService)
        {
            _tweetService = tweetService;
        }
        /// <summary>
        /// Get all tweets for all users
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("api/[controller]/all")]
        public IActionResult GetAllTweets()
        {
            var tweetList = _tweetService.GetAllTweets();
            return Ok(tweetList);
        }

        /// <summary>
        /// Get all tweets for an user using username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        
        [HttpGet("api/[controller]/{username}")]
        public IActionResult GetAllTweetsOfUser([FromRoute]string username)
        {
            var tweetsOfUser = _tweetService.GetAllTweetsOfUser(username);
            return Ok(tweetsOfUser);
        }

        /// <summary>
        /// Add a tweet for an user
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        
        [HttpPost("api/[controller]/{username}/add")]
        public IActionResult AddTweet([FromBody] Tweet tweet, [FromRoute] string username)
        {
           var result = _tweetService.AddTweet(tweet, username);
           return Created("", result);
        }

        /// <summary>
        /// Update a users tweet using tweet id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>        
        [HttpPut("api/[controller]/{username}/update/{id}")]
        public void UpdateTweet([FromRoute] int id, [FromRoute] string username, [FromBody]Tweet updatedTweet)
        {
        }

        /// <summary>
        ///  Delete a user's tweet using tweet id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("api/[controller]/{username}/delete/{id}")]
        public void DeleteTweet([FromRoute] int id)
        {
        }

        /// <summary>
        /// Like a users tweet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("api/[controller]/{username}/like/{id}")]
        public void LikeTweet([FromRoute] int id, [FromRoute] string likedByUsername)
        {
        }

        /// <summary>
        /// Reply to a tweet from an user
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/[controller]/{username}/reply/{id}")]
        public IActionResult ReplyTweet([FromRoute] int id, [FromBody] Tweet tweet)
        {
            return Ok();
        }

       
    }
}
