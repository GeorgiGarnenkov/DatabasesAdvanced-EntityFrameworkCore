using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class Employee
    {
        public Employee()
        {
            this.Orders = new List<Order>();
        }

        //	Id – integer, Primary Key
        //	Name – text with min length 3 and max length 30 (required)
        //	Age – integer in the range[15, 80] (required)
        //	PositionId - integer, foreign key
        //	Position – the employee’s position(required)
        //  Orders – the orders the employee has processed

        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(15, 80)]
        public int Age { get; set; }

        public int PositionId { get; set; }

        [Required]
        public Position Position { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}