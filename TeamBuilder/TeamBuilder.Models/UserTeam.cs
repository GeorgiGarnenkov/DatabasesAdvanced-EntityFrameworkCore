using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilder.Models
{
    public class UserTeam
    {
        [Range(0, Int32.MaxValue)]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Range(0, Int32.MaxValue)]
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public Team Team { get; set; }

    }
}