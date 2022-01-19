using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TweetService.Models
{
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public int? TweetId { get; set; }
        public string Message { get; set; }
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public int DislikeCount { get; set; }
        public DateTime PostedOn { get; set; }
    }
}
