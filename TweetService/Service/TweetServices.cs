using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetService.Models;
using TweetService.Repository;

namespace TweetService.Service
{
    public class TweetServices:ITweetServices
    {
        private readonly ITweetRepository _tweetRepository;
        public TweetServices(ITweetRepository tweetRepository)
        {
            _tweetRepository = tweetRepository;
        }
        public bool AddTweet(Tweet tweet, string userName)
        {
            var addResult = _tweetRepository.AddTweet(tweet, userName);
            return addResult;
        }

        public bool DeleteTweet(string tweetId)
        {
            var deleteResult = _tweetRepository.DeleteTweet(tweetId);
            return deleteResult;
        }
        public IEnumerable<Tweet> GetAllTweets()
        {
            return _tweetRepository.GetAllTweets();
        }

        public IEnumerable<Tweet> GetAllTweetsOfUser(string userName)
        {
            return _tweetRepository.GetAllTweetsOfUser(userName);
        }
        public bool LikeTweet(string tweetId)
        {
            var likeResult = _tweetRepository.LikeTweet(tweetId);
            return likeResult;
        }
        public bool UpdateTweet(string tweetId, string userName, Tweet updatedTweet)
        {
            var updateResult = _tweetRepository.UpdateTweet(tweetId, userName, updatedTweet);
            return updateResult;
        }
   }
}
