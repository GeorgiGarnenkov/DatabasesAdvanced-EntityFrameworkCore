using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilder.Models
{
    public class Invitation
    {
        [Key]
        [Range(0, Int32.MaxValue)]
        public int Id { get; set; }

        [Range(0, Int32.MaxValue)]
        [ForeignKey("InvitedUser")]
        public int InvitedUserId { get; set; }
        public User InvitedUser { get; set; }

        [Range(0, Int32.MaxValue)]
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public bool IsActive { get; set; } = true;

    }
}