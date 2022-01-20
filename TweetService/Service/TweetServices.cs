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
        public async Task<Tweet> AddTweet(Tweet tweet, string userName)
        {
            var addResult = await _tweetRepository.AddTweet(tweet, userName);
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
        public bool LikeTweet(string tweetId, string userName)
        {
            var likeResult = _tweetRepository.LikeTweet(tweetId, userName);
            return likeResult;
        }
        public bool UpdateTweet(string tweetId, string userName, Tweet updatedTweet)
        {
            var updateResult = _tweetRepository.UpdateTweet(tweetId, userName, updatedTweet);
            return updateResult;
        }
   }
}
