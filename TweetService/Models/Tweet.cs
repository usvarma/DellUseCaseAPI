using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TweetService.Models
{
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int? TweetId { get; set; }
        [MaxLength(144)]
        public string Message { get; set; }
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        public IEnumerable<String> LikedByUsers { get; set; }
        public IEnumerable<String> RepliedByUsers { get; set; }
        public int? ParentId { get; set; }
        public IEnumerable<Tweet> Replies { get; set; }
        [MaxLength(50)]
        public string Tag { get; set; }
        public DateTime PostedOn { get; set; }
    }
}
