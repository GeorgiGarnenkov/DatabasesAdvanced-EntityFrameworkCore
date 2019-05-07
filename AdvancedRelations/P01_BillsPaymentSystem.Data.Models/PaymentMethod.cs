using System;
using System.ComponentModel.DataAnnotations;
using P01_BillsPaymentSystem.Data.Models.Attributes;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }    // PK

        public int UserId { get; set; }
        public User User { get; set; }

        [Xor(nameof(BankAccountId))] // <----  !!!
        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }

        public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        

        public PaymentMethodType Type { get; set; }     // enum (BankAccount, CreditCard)
    }
}