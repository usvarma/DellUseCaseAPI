using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetService.Models;
using TweetService.TweetDBContext;

namespace TweetService.Repository
{
    public class TweetRepository : ITweetRepository
    {
        private readonly TweetDbContext _tweetDbContext;
        public TweetRepository(TweetDbContext tweetDbContext)
        {
            _tweetDbContext = tweetDbContext;
        }
        public async Task<Tweet> AddTweet(Tweet tweet, string userName)
        {
            var tweetList = _tweetDbContext.Tweets.AsQueryable();
            if (tweetList.Any())
            {
                tweet.TweetId = tweetList.Max(t => t.TweetId) + 1;
            }
            else
            {
                tweet.TweetId = 1;
            }
            tweet.PostedOn = DateTime.Now;
            await _tweetDbContext.Tweets.InsertOneAsync(tweet);
            return tweet;
        }
        public bool DeleteTweet(string tweetId)
        {
            _ = int.TryParse(tweetId, out var intTweetId);
            if(intTweetId != 0)
            {
                var deletedTweet = _tweetDbContext.Tweets.FindOneAndDelete(tweet => tweet.TweetId == intTweetId);
                return deletedTweet != null;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<Tweet> GetAllTweets()
        {
            return _tweetDbContext.Tweets.Find(tweet => tweet.TweetId != null && tweet.Username != null).ToList();
        }
        public IEnumerable<Tweet> GetAllTweetsOfUser(string userName)
        {
            return _tweetDbContext.Tweets.Find(tweet => tweet.TweetId != null && tweet.Username == userName).ToList();
        }
        public bool LikeTweet(string tweetId, string username)
        {
            _ = int.TryParse(tweetId, out var intTweetId);
            if (intTweetId != 0)
            {
                var tweetToUpdate = _tweetDbContext.Tweets.Find(tweet => tweet.TweetId == intTweetId).ToList();
                if (tweetToUpdate == null || tweetToUpdate.Count() <= 0)
                {
                    return false;
                }
                else
                {
                    tweetToUpdate[0].LikedByUsers.Append(username);
                    var updatedTweet = _tweetDbContext.Tweets.ReplaceOne(tweet => tweet.TweetId == intTweetId, tweetToUpdate[0]);
                    return updatedTweet != null;
                }

            }
            else
            {
                return false;
            }
        }
        public bool UpdateTweet(string tweetId, string userName, Tweet valueToUpdate)
        {
            _ = int.TryParse(tweetId, out var intTweetId);
            if (intTweetId != 0)
            {
                var tweetToUpdate = _tweetDbContext.Tweets.Find(tweet => tweet.TweetId == intTweetId).ToList();
                if (tweetToUpdate == null || tweetToUpdate.Count <= 0)
                {
                    return false;
                }
                else
                {
                    var updatedTweet = _tweetDbContext.Tweets.ReplaceOne(tweet => tweet.TweetId == intTweetId, valueToUpdate);
                    return updatedTweet != null;
                }

            }
            else
            {
                return false;
            }
        }
        public bool ReplyTweet(string tweetId, string username)
        {
            _ = int.TryParse(tweetId, out var intTweetId);
            if (intTweetId != 0)
            {
                var tweetToUpdate = _tweetDbContext.Tweets.Find(tweet => tweet.TweetId == intTweetId).ToList();
                if (tweetToUpdate == null || tweetToUpdate.Count <= 0)
                {
                    return false;
                }
                else
                {
                    tweetToUpdate[0].RepliedByUsers.Append(username);
                    var updatedTweet = _tweetDbContext.Tweets.ReplaceOne(tweet => tweet.TweetId == intTweetId, tweetToUpdate[0]);
                    return updatedTweet != null;
                }

            }
            else
            {
                return false;
            }
        }


    }
}
