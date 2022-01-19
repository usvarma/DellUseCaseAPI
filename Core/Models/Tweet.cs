using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;


namespace Core.Models
{
    public class Tweet
    {
        [BsonId]
        [Required]
        public int? TweetId { get; set; }
        public string Message { get; set; }
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        public int LikeCount { get; set; }
        public int ReplyCount { get; set; }
        public int DislikeCount { get; set; }
        [Required]
        public DateTime PostedOn { get; set; }
    }
}
