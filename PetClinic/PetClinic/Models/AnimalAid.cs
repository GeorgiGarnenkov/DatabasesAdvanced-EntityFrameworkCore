using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.Models
{
    public class AnimalAid
    {
        //-	Id                  – integer, Primary Key
        //-	Name                – text with min length 3 and max length 30 (required, UNIQUE)
        //-	Price               – decimal (non-negative, minimum value: 0.01, required)
        //-	AnimalAidProcedures – collection of type ProcedureAnimalAid

        [Key]
        public int Id { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [Required]
        public decimal Price { get; set; }

        public ICollection<ProcedureAnimalAid> AnimalAidProcedures { get; set; }

    }
}