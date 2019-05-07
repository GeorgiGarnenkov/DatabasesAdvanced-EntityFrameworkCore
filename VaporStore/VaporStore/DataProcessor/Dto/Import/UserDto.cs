using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UserDto
    {
        [Required]
        [RegularExpression(@"^\b[A-Z][a-z]+\b\ \b[A-Z][a-z]+\b$")]
        public string FullName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        public CardDto[] Cards { get; set; }
    }
}