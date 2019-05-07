using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeamBuilder.Models.Enums;

namespace TeamBuilder.Models
{
    public class User
    {
        public User()
        {
            this.CreatedEvents = new List<Event>();
            this.MemberOf = new List<UserTeam>();
            this.CreatedUserTeams = new List<UserTeam>();
            this.ReceivedInvitations = new List<Invitation>();
        }
        
        [Key]
        [Range(0, Int32.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Username { get; set; } // Unique 
        
        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }


        [RegularExpression(@"^(?:(?=.*\d)(?=.*[A-Z]).*)$")]
        [StringLength(30, MinimumLength = 6)]
        [Required]
        public string Password { get; set; } // Should contain one digit and one uppercase letter, Required

        public Gender Gender { get; set; } // Could be: 'Male' or 'Female'

        [Range(0, Int32.MaxValue)]
        public int Age { get; set; }

        public bool IsDeleted { get; set; }
        
        public ICollection<Event> CreatedEvents { get; set; }
        public ICollection<UserTeam> MemberOf { get; set; }
        public ICollection<UserTeam> CreatedUserTeams
        { get; set; }
        public ICollection<Invitation> ReceivedInvitations { get; set; }
    }
}
