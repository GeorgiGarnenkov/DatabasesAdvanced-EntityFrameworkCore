using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilder.Models
{
    public class Team
    {
        public Team()
        {
            this.Members = new List<UserTeam>();
            this.EventsParticipated = new List<EventTeam>();
            this.Invitations = new List<Invitation>();
        }

        [Key]
        [Range(0, Int32.MaxValue)]
        public int Id { get; set; }
        
        [MaxLength(25)]
        [Required]
        public string Name { get; set; } //Unique, Required

        [MaxLength(32)]
        public string Description { get; set; }

        [StringLength(3, MinimumLength = 3)]
        [Required]
        public string Acronym { get; set; }

        [Range(0, Int32.MaxValue)]
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<Invitation> Invitations { get; set; }

        public ICollection<UserTeam> Members { get; set; }
        public ICollection<EventTeam> EventsParticipated { get; set; }
       
    }
}