using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TweetService.Models;
using UserService.Models;

namespace TweetService.TweetDBContext
{
    public class TweetDbContext
    {
        readonly MongoClient mongoClient;
        readonly IMongoDatabase mongoDatabase;
        public TweetDbContext(IConfiguration configuration)
        {
            mongoClient = new MongoClient(configuration.GetSection("TweetsDatabase:ConnectionString").Value);
            mongoDatabase = mongoClient.GetDatabase(configuration.GetSection("TweetsDatabase:DatabaseName").Value);
        }
        
        //create tweet collections
        public IMongoCollection<Tweet> Tweets => mongoDatabase.GetCollection<Tweet>("Tweets");
        public IMongoCollection<User> Users => mongoDatabase.GetCollection<User>("Users");
    }
}
