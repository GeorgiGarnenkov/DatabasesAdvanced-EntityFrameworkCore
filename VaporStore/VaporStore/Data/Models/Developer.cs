using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Developer
    {
        //    •	Id – integer, Primary Key
        //•	Name – text(required)
        //•	Games - collection of type Game
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}