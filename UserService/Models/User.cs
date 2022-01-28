using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Url]
        public string AvatarUrl { get; set; }
        public HashedPassword PasswordHashed{get;set;}
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
