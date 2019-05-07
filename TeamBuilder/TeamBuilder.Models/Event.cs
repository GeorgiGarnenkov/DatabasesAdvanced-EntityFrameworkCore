using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml;

namespace TeamBuilder.Models
{
    public class Event
    {
        private DateTime endDate;

        public Event()
        {
            this.ParticipatingEventTeams = new List<EventTeam>();
        }

        [Key]
        [Range(0, Int32.MaxValue)]
        public int Id { get; set; }

        [MaxLength(25)]
        [Required]
        public string Name { get; set; } // unicode

        [MaxLength(250)]
        public string Description { get; set; } // unicode

        public DateTime StartDate { get; set; }

        public DateTime EndDate
        {
            get => this.endDate;
            set
            {
                if (value <= this.StartDate)
                {
                    throw new ArgumentException("Start date should be before end date.");
                }

                this.endDate = value;
            }
        }

        [Range(0, Int32.MaxValue)]
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<EventTeam> ParticipatingEventTeams { get; set; }
    }
}