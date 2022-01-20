using System.Collections.Generic;
using System.Threading.Tasks;
using TweetService.Models;

namespace TweetService.Repository
{
    public interface ITweetRepository
    {
        IEnumerable<Tweet> GetAllTweets();
        IEnumerable<Tweet> GetAllTweetsOfUser(string userName);
        public Task<Tweet> AddTweet(Tweet tweet, string userName);
        bool UpdateTweet(string tweetId, string userName, Tweet updatedTweet);
        bool DeleteTweet(string tweetId);
        bool LikeTweet(string tweetId, string username);
        public bool ReplyTweet(string tweetId, string username);
    }
}
