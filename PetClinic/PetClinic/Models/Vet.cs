using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.Models
{
    public class Vet
    {
        //-	Id          – integer, Primary Key
        //-	Name        – text with min length 3 and max length 40 (required)
        //-	Profession  – text with min length 3 and max length 50 (required)
        //-	Age         – integer number, minimum value of 22 years and maximum 65 (required)
        //-	PhoneNumber – required, UNIQUE, make sure it matches one of the following requirements:
                    //«	either starts with +359 and is followed by 9 digits
                    //«	or consists of exactly 10 digits, starting with 0
        //-	Procedures  – the procedures, performed by the vet
        [Key]
        public int Id { get; set; }

        [StringLength(40, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Profession { get; set; }

        [Range(22, 65)]
        [Required]
        public int Age { get; set; }

        [RegularExpression(@"^\+359[0-9]{9}$|^0[0-9]{9}$")]
        [Required]
        public string PhoneNumber { get; set; }

        public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
    }
}