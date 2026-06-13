using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookRen.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        
        public string? Username { get ; set; }
        
        [Required]
        [MinLength(length: 6)]
        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}
