using System;
using System.ComponentModel.DataAnnotations;
using PetClinic.Models;

namespace PetClinic.DataProcessor.Dto.Import
{
    public class PassportDto
    {
        [RegularExpression("^[A-Za-z]{7}[0-9]{3}$")]
        public string SerialNumber { get; set; }
        
        [RegularExpression(@"^\+359[0-9]{9}$|^0[0-9]{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }
}