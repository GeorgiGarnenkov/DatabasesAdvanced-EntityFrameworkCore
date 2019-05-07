using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Tag
    {
        //    •	Id – integer, Primary Key
        //•	Name – text(required)
        //•	GameTags - collection of type GameTag
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();

    }
}