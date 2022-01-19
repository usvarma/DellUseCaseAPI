using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class User
    {
        [EmailAddress]
        [Required]
        public string Username { get; set; }
        [Required]
        public string Handle { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
