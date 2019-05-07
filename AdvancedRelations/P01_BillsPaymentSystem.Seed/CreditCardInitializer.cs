using System;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Seed
{
    public class CreditCardInitializer
    {
        public static CreditCard[] GeCreditCards()
        {
            CreditCard[] creditCards = new CreditCard[]
            {
                new CreditCard{ Limit = 5000, ExpirationDate = new DateTime(2020, 12, 1)},
                new CreditCard{ Limit = 10000, ExpirationDate = new DateTime(2020, 01, 1)},
                new CreditCard{ Limit = 15000, ExpirationDate = new DateTime (2020, 02, 1)},
                new CreditCard{ Limit = 12000, ExpirationDate = new DateTime (2020, 05, 1)},
                new CreditCard{ Limit = 7000, ExpirationDate = new DateTime (2020, 07, 1)},
                new CreditCard{ Limit = 2000, ExpirationDate = new DateTime(2020, 10, 1)}
            };

            return creditCards;
        }
    }
}