using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [JsonIgnore]
        public int? UserId { get; set; }
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Handle { get; set; }
        [Required]
        public string Password { get; set; }
        public HashedPassword PasswordHashed{get;set;}
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
