using System.Collections.Generic;
using System.Threading.Tasks;
using TweetService.Models;

namespace TweetService.Repository
{
    public interface ITweetRepository
    {
        Task<IEnumerable<Tweet>> GetAllTweetsAsync();
        Task<IEnumerable<Tweet>> GetAllTweetsOfUserAsync(string userName);
        Task<Tweet> AddTweetAsync(Tweet tweet, string userName);
        Task UpdateTweetAsync(int? tweetId, string userName, Tweet updatedTweet);
        Task DeleteTweetAsync(int? tweetId, string username);
        Task LikeTweetAsync(int? tweetId, string username);
        Task ReplyTweetAsync(int? parentTweetId, string repliedByUsername, Tweet replyTweet);
    }
}
