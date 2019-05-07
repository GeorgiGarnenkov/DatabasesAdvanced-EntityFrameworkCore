using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        //•	Id             – integer, Primary Key
        //•	CellNumber    – integer in the range[1, 1000] (required)
        //•	HasWindow      – bool (required)
        //•	DepartmentId - integer, foreign key
        //•	Department   – the cell's department (required)
        //•	Prisoners    - collection of type Prisoner

        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1,1000)]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Prisoner> Prisoners { get; set; } = new List<Prisoner>();
    }
}