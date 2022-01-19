using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetService.Models;

namespace TweetService.Service
{
    public interface ITweetServices
    {
        IEnumerable<Tweet> GetAllTweets();
        IEnumerable<Tweet> GetAllTweetsOfUser(string userName);
        bool AddTweet(Tweet tweet, string userName);
        bool UpdateTweet(string tweetId, string userName, Tweet updatedTweet);
        bool DeleteTweet(string tweetId);
        bool LikeTweet(string tweetId);
    }
}
