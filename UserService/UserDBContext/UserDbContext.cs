using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UserService.Models;

namespace UserService.UserDBContext
{
    public class UserDbContext
    {
        readonly MongoClient mongoClient;
        readonly IMongoDatabase mongoDatabase;
        public UserDbContext(IConfiguration configuration)
        {
            mongoClient = new MongoClient(configuration.GetSection("TweetsDatabase:ConnectionString").Value);
            mongoDatabase = mongoClient.GetDatabase(configuration.GetSection("TweetsDatabase:DatabaseName").Value);
            var usersCollection = mongoDatabase.GetCollection<User>("Users");
            //var tweetsCollection = mongoDatabase.GetCollection<User>("Tweets");
        }

        public IMongoCollection<User> Users => mongoDatabase.GetCollection<User>("Users");
    }
}
