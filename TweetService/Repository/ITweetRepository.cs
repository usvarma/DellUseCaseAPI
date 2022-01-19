using System.Collections.Generic;
using TweetService.Models;

namespace TweetService.Repository
{
    public interface ITweetRepository
    {
        IEnumerable<Tweet> GetAllTweets();
        IEnumerable<Tweet> GetAllTweetsOfUser(string userName);
        bool AddTweet(Tweet tweet, string userName);
        bool UpdateTweet(string tweetId, string userName, Tweet updatedTweet);
        bool DeleteTweet(string tweetId);
        bool LikeTweet(string tweetId);
    }
}
