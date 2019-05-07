using System.ComponentModel.DataAnnotations;

namespace PetClinic.Models
{
    public class ProcedureAnimalAid
    {
        //-	ProcedureId  – integer, Primary Key
        //-	Procedure    – the animal aid’s procedure(required)
        //-	AnimalAidId  – integer, Primary Key
        //-	AnimalAid    – the procedure’s animal aid(required)

        [Required]
        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; }

        [Required]
        public int AnimalAidId { get; set; }
        public AnimalAid AnimalAid { get; set; }
    }
}