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
        
        public async Task<Tweet> AddTweetAsync(Tweet tweet, string userName)
        {
            var addResult = await _tweetRepository.AddTweetAsync(tweet, userName);
            return addResult;
        }
        public async Task DeleteTweetAsync(int? tweetId)
        {
            await _tweetRepository.DeleteTweetAsync(tweetId);
        }
        public async Task<IEnumerable<Tweet>> GetAllTweetsAsync()
        {
            return await _tweetRepository.GetAllTweetsAsync();
        }
        public async Task<IEnumerable<Tweet>> GetAllTweetsOfUserAsync(string userName)
        {
            var tweetList = await _tweetRepository.GetAllTweetsOfUserAsync(userName);
            return tweetList;
        }
        public async Task LikeTweetAsync(int? tweetId, string userName)
        {
            await _tweetRepository.LikeTweetAsync(tweetId, userName);
        }
        public async Task ReplyTweetAsync(int? parentTweetId, string repliedByUsername, Tweet replyTweet)
        {
            await _tweetRepository.ReplyTweetAsync(parentTweetId, repliedByUsername, replyTweet);
        }
        public async Task UpdateTweetAsync(int? tweetId, string userName, Tweet newTweet)
        {
            await _tweetRepository.UpdateTweetAsync(tweetId, userName, newTweet);
        }
   }
}
