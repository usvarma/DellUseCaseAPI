using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetService.Models;

namespace TweetService.Service
{
    public interface ITweetServices
    {
        Task<IEnumerable<Tweet>> GetAllTweetsAsync();
        Task<IEnumerable<Tweet>> GetAllTweetsOfUserAsync(string userName);
        Task<Tweet> AddTweetAsync(Tweet tweet, string userName);
        Task UpdateTweetAsync(int? tweetId, string userName, Tweet newTweet);
        Task DeleteTweetAsync(int? tweetId, string username);
        Task LikeTweetAsync(int? tweetId, string userName);
        Task ReplyTweetAsync(int? parentTweetId, string repliedByUsername, Tweet replyTweet);
    }
}
