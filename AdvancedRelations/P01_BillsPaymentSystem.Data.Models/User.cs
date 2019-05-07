using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using P01_BillsPaymentSystem.Data.Models.Attributes;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }    // (up to 50 characters, unicode)

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }   // (up to 50 characters, unicode)

        [Required]
        [MaxLength(80)]
        [NonUnicode]
        public string Email { get; set; }   // (up to 80 characters, non-unicode)

        [Required]
        [MaxLength(25)]
        [NonUnicode]
        public string Password { get; set; }  // (up to 25 characters, non-unicode)

        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
