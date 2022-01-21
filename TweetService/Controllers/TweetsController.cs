using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TweetService.Models;
using TweetService.Service;

namespace TweetService.Controllers
{
    [Route("")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly ILogger<TweetsController> _logger;
        private readonly ITweetServices _tweetService;
        public TweetsController(ITweetServices tweetService, ILogger<TweetsController> logger)
        {
            _tweetService = tweetService;
            _logger = logger;
        }
        
        /// <summary>
        /// Get all tweets for all users
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("api/v1.0/[controller]/all")]
        public async Task<IActionResult> GetAllTweetsAsync()
        {
            try
            {
                var tweetList = await _tweetService.GetAllTweetsAsync();
                _logger.LogInformation($"Retrieved all tweets successfully");
                return Ok(tweetList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving tweets. Exception is {ex}");
                return StatusCode(500, $"Server error occurred while trying to retrieve all tweets");
            }
        }

        /// <summary>
        /// Get all tweets for an user using username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        
        [HttpGet("api/v1.0/[controller]/{username}")]
        public async Task<IActionResult> GetAllTweetsOfUserAsync([FromRoute]string username)
        {
            try
            {
                var tweetsOfUser = await _tweetService.GetAllTweetsOfUserAsync(username);
                _logger.LogInformation($"Retrieved all tweets for user: {username}");
                return Ok(tweetsOfUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving tweets. Exception is {ex}");
                return StatusCode(500, $"Server error occurred while trying to retrieve tweets for {username}");
            }
        }

        /// <summary>
        /// Add a tweet for an user
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        
        [HttpPost("api/v1.0/[controller]/{username}/add")]
        public async Task<IActionResult> AddTweetAsync([FromBody] Tweet tweet, [FromRoute] string username)
        {
            try
            {
                var newTweet = await _tweetService.AddTweetAsync(tweet, username);
                _logger.LogInformation($"Added a new tweet for {username}");
                return Created("", newTweet);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding a tweet {tweet}. Exception is {ex}");
                return StatusCode(500, $"Server error occurred while trying to add tweet");
            }
        }

        /// <summary>
        /// Update a users tweet using tweet id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>        
        [HttpPut("api/v1.0/[controller]/{username}/update/{id}")]
        public async Task<IActionResult> UpdateTweetAsync([FromRoute] int? id, [FromRoute] string username, [FromBody]Tweet newTweet)
        {
            try
            {
                await _tweetService.UpdateTweetAsync(id,username, newTweet);
                _logger.LogInformation($"Updated tweet with id: {id} for {username}");
                return Ok($"Updated tweet with id: {id} for {username}");
            }
            catch(ArgumentException argEx)
            {
                _logger.LogError($"Error while updating a tweet with id: {id}. Exception is {argEx}");
                return StatusCode(400, $"{argEx.Message}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while updating a tweet with id: {id}. Exception is {ex}");
                return StatusCode(500, $"Server error occurred while trying to update tweet with id: {id}. Exception Msg is {ex.Message}");
            }
        }

        /// <summary>
        ///  Delete a user's tweet using tweet id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("api/v1.0/[controller]/{username}/delete/{id}")]
        public async Task<IActionResult> DeleteTweetAsync([FromRoute] int? id)
        {
            try
            {
                await _tweetService.DeleteTweetAsync(id);
                _logger.LogInformation($"Deleted tweet with id: {id} successfully");
                return Ok($"Deleted tweet with id: {id} successfully");
            } 
            catch (ArgumentException argEx)
            {
                _logger.LogError($"Error while deleting a tweet with id: {id}. Exception is {argEx}");
                return StatusCode(400, $"{argEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting a tweet with id: {id}. Exception is {ex}");
                return StatusCode(500, $"Server error occurred while trying to delete tweet with id: {id}. Exception Msg is {ex.Message}");
            }
        }

        /// <summary>
        /// Like a users tweet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("api/v1.0/[controller]/{username}/like/{id}")]
        public async Task<IActionResult> LikeTweetAsync([FromRoute] int? id, [FromRoute] string likedByUsername)
        {
            try
            {
                await _tweetService.LikeTweetAsync(id, likedByUsername);
                _logger.LogInformation($"Liked tweet with id: {id} successfully");
                return Ok($"Liked tweet with id: {id} successfully");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError($"Error while liking a tweet with id: {id}. Exception is {argEx}");
                return StatusCode(400, $"{argEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting a tweet with id: {id}. Exception is {ex}");
                return StatusCode(500, $"Server error occurred while trying to like tweet with id: {id}. Exception Msg is {ex.Message}");
            }
        }

        /// <summary>
        /// Reply to a tweet from an user
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v1.0/[controller]/{username}/reply/{id}")]
        public async Task<IActionResult> ReplyTweetAsync([FromRoute] int id, [FromRoute] string username, [FromBody] Tweet replyTweet)
        {
            try
            {
                await _tweetService.ReplyTweetAsync(id, username, replyTweet);
                _logger.LogInformation($"Replied to tweet with id: {id} successfully");
                return Ok($"REplied to tweet with id: {id} successfully");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError($"Error while replying to a tweet with id: {id}. Exception is {argEx}");
                return StatusCode(400, $"{argEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while replying to a tweet with id: {id}. Exception is {ex}");
                return StatusCode(500, $"Server error occurred while trying to reply to a tweet with id: {id}. Exception Msg is {ex.Message}");
            }
        }
       
    }
}
