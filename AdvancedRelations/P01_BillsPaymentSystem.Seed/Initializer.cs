using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using P01_BillsPaymentSystem.Data;

namespace P01_BillsPaymentSystem.Seed
{
    public class Initializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            InsertUsers(context);
            InsertCreditCards(context);
            InsertBankAccounts(context);
            InsertPaymentMethods(context);
        }

        private static void InsertUsers(BillsPaymentSystemContext context)
        {
            var users = UserInitializer.GetUsers();

            foreach (var user in users)
            {
                if (IsValid(user))
                {
                    context.Users.Add(user);
                }
            }

            context.SaveChanges();
        }

        private static void InsertCreditCards(BillsPaymentSystemContext context)
        {
            var creditCards = CreditCardInitializer.GeCreditCards();

            foreach (var card in creditCards)
            {
                if (IsValid(card))
                {
                    context.CreditCards.Add(card);
                }
            }

            context.SaveChanges();
        }

        private static void InsertBankAccounts(BillsPaymentSystemContext context)
        {
            var bankAccounts = BankAccountInitializer.GetBankAccounts();

            foreach (var account in bankAccounts)
            {
                if (IsValid(account))
                {
                    context.BankAccounts.Add(account);
                }
            }

            context.SaveChanges();
        }

        private static void InsertPaymentMethods(BillsPaymentSystemContext context)
        {
            var paymentMethods = PaymentMethodInitializer.GetPaymentMethods();

            foreach (var method in paymentMethods)
            {
                if (IsValid(method))
                {
                    context.PaymentMethods.Add(method);
                }
            }

            context.SaveChanges();
        }

        private static bool IsValid(object obj)
        {
            var context = new ValidationContext(obj);
            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, context, result, true);
        }
    }
}
