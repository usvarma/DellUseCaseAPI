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
        public async Task<Tweet> AddTweetAsync(Tweet tweet, string userName)
        {
            if(tweet == null || string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException($"Invalid arguments for adding a tweet. Username is: {userName} and tweet is: {tweet}");
            }
            
            var tweetList = _tweetDbContext.Tweets.AsQueryable();
            var userList = _tweetDbContext.Users.AsQueryable();
            var isValidUsername = _tweetDbContext.Users.Find(user => user.EmailAddress == userName).Any();

            if (isValidUsername)
            {
                throw new ArgumentException($"Cannot add tweet for an user who doesn't exist with {userName}");
            }
            
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
        public async Task DeleteTweetAsync(int? tweetId)
        {
            if (tweetId == null)
            {
                throw new ArgumentException($"Invalid tweet id: {tweetId}");
            }
            else
            {
                var tweetList = _tweetDbContext.Tweets.Find(tweet => tweet.TweetId == tweetId).ToList();
                if (!tweetList.Any())
                {
                    throw new ArgumentException($"Cannot find tweet with id: {tweetId} to delete");
                }
                await _tweetDbContext.Tweets.FindOneAndDeleteAsync(tweet => tweet.TweetId == tweetId);
            }
        }
        public async Task<IEnumerable<Tweet>> GetAllTweetsAsync()
        {
            return await _tweetDbContext.Tweets.Find(tweet => tweet.TweetId != null && tweet.Username != null).ToListAsync();
        }
        public async Task<IEnumerable<Tweet>> GetAllTweetsOfUserAsync(string userName)
        {
            return string.IsNullOrWhiteSpace(userName) ? new List<Tweet>() : await _tweetDbContext.Tweets.Find(tweet => tweet.TweetId != null && tweet.Username == userName).ToListAsync();
        }
        public async Task LikeTweetAsync(int? tweetId, string username)
        {
            if (tweetId == null || string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"Invalid tweet id or username. Tweet id is {tweetId} and username is {username}");
            }

            var tweetToUpdate = _tweetDbContext.Tweets.Find(tweet => tweet.TweetId == tweetId).ToList();
            var likedUser = _tweetDbContext.Users.Find(user => user.Username == username);

            if (!likedUser.Any())
            {
                throw new ArgumentException($"Cannot find user with username: {username} when liking tweets");
            }

            if (!tweetToUpdate.Any())
            {
                throw new ArgumentException($"Cannot find tweet with id: {tweetId} to like");
            }

            if (tweetToUpdate.FirstOrDefault().Username == username)
            {
                throw new ArgumentException($"User cannot like own tweet");
            }

            var newLikedByUsers = tweetToUpdate[0].LikedByUsers.Append(username);
            tweetToUpdate[0].LikedByUsers = newLikedByUsers;
            await _tweetDbContext.Tweets.ReplaceOneAsync(tweet => tweet.TweetId == tweetId, tweetToUpdate[0]);
        }
        public async Task UpdateTweetAsync(int? tweetId, string userName, Tweet updatedTweet)
        {
            if (tweetId == null || string.IsNullOrWhiteSpace(userName) || updatedTweet == null)
            {
                throw new ArgumentException($"Invalid arguments to update tweet. Tweet id: {tweetId}, username: {userName} and updated tweet value: {updatedTweet}");
            }

            var updatedByUser = _tweetDbContext.Users.Find(user => user.Username == userName);

            if (!updatedByUser.Any())
            {
                throw new ArgumentException($"Cannot find user with username: {userName} when updating a tweet");
            }

            var tweetToUpdate = _tweetDbContext.Tweets.Find(tweet => tweet.TweetId == tweetId).ToList();
            if (!tweetToUpdate.Any())
            {
                throw new ArgumentException($"Cannot find tweet with id: {tweetId} to update");
            }
            await _tweetDbContext.Tweets.ReplaceOneAsync(tweet => tweet.TweetId == tweetId, updatedTweet);
        }
        public async Task ReplyTweetAsync(int? parentTweetId, string repliedByUsername, Tweet replyTweet)
        {
            if (parentTweetId == null || string.IsNullOrWhiteSpace(repliedByUsername) || replyTweet == null)
            {
                throw new ArgumentException($"Invalid arguments while replying a tweet. Parent tweet id: {parentTweetId}, replying user: {repliedByUsername} and replied tweet {replyTweet}");
            }

            var repliedByUser = _tweetDbContext.Users.Find(user => user.Username == repliedByUsername);

            if (!repliedByUser.Any())
            {
                throw new ArgumentException($"Cannot find user with username: {repliedByUsername} when replying to a tweet");
            }

            var tweetToReply = _tweetDbContext.Tweets.Find(tweet => tweet.TweetId == parentTweetId).ToList();
            if (!tweetToReply.Any())
            {
                throw new ArgumentException($"Cannot find tweet id: {parentTweetId} to reply");
            }

            var repliedByUsers = tweetToReply[0].RepliedByUsers.Append(repliedByUsername);
            var tweetReplies = tweetToReply[0].Replies.Append(replyTweet);
            tweetToReply[0].Replies = tweetReplies;
            tweetToReply[0].RepliedByUsers = repliedByUsers;

            await _tweetDbContext.Tweets.ReplaceOneAsync(tweet => tweet.TweetId == parentTweetId, tweetToReply[0]);
        }
    }
}
