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
        private readonly TweetDbContext _tweetDbcontext;
        public TweetRepository(TweetDbContext tweetDbContext)
        {
            _tweetDbcontext = tweetDbContext;
        }
        public bool AddTweet(Tweet tweet, string userName)
        {
            var tweetList = _tweetDbcontext.Tweets.AsQueryable();
            if (tweetList.Count() > 0)
            {
                tweet.TweetId = tweetList.Max(t => t.TweetId) + 1;
            }
            else
            {
                tweet.TweetId = 1;
            }
            tweet.PostedOn = DateTime.Now;
            _tweetDbcontext.Tweets.InsertOne(tweet);
            return true;
        }
        public bool DeleteTweet(string tweetId)
        {
            Int32.TryParse(tweetId, out var intTweetId);
            if(intTweetId != 0)
            {
                var deletedTweet = _tweetDbcontext.Tweets.FindOneAndDelete(tweet => tweet.TweetId == intTweetId);
                return deletedTweet != null;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<Tweet> GetAllTweets()
        {
            return _tweetDbcontext.Tweets.Find(tweet => tweet.TweetId != null && tweet.Username != null).ToList();
        }
        public IEnumerable<Tweet> GetAllTweetsOfUser(string userName)
        {
            return this._tweetDbcontext.Tweets.Find(tweet => tweet.TweetId != null && tweet.Username == userName).ToList();
        }
        public bool LikeTweet(string tweetId)
        {
            Int32.TryParse(tweetId, out var intTweetId);
            if (intTweetId != 0)
            {
                var tweetToUpdate = _tweetDbcontext.Tweets.Find(tweet => tweet.TweetId == intTweetId).ToList();
                if (tweetToUpdate == null || tweetToUpdate.Count() <= 0)
                {
                    return false;
                }
                else
                {
                    tweetToUpdate[0].LikeCount += 1;
                    var updatedTweet = _tweetDbcontext.Tweets.ReplaceOne(tweet => tweet.TweetId == intTweetId, tweetToUpdate[0]);
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
            Int32.TryParse(tweetId, out var intTweetId);
            if (intTweetId != 0)
            {
                var tweetToUpdate = _tweetDbcontext.Tweets.Find(tweet => tweet.TweetId == intTweetId).ToList();
                if (tweetToUpdate == null || tweetToUpdate.Count() <= 0)
                {
                    return false;
                }
                else
                {
                    var updatedTweet = _tweetDbcontext.Tweets.ReplaceOne(tweet => tweet.TweetId == intTweetId, valueToUpdate);
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
