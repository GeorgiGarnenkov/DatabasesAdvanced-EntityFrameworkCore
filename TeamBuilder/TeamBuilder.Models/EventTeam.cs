using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamBuilder.Models
{
    public class EventTeam
    {
        [Range(0, Int32.MaxValue)]
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Range(0, Int32.MaxValue)]
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        
    }
}