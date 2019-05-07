using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.App
{
    using Data;
    using Seed;

    public class StartUp
    {
        public static void Main()
        {
            using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            {
                //User user = GetUser(context);
                //GetInfo(user);

                
            }


        }

        private static void PayBills(User user, decimal amount)
        {
            var bankAccountsTotalSum = user.PaymentMethods.Where(x => x.BankAccount != null)
                .Sum(x => x.BankAccount.Balance);
            var creditCardsTotalSum = user.PaymentMethods.Where(x => x.CreditCard != null)
                .Sum(x => x.CreditCard.LimitLeft);

            var totalSumAmount = bankAccountsTotalSum + creditCardsTotalSum;

            if (totalSumAmount >= amount)
            {
                var bankAccounts = user.PaymentMethods
                    .Where(x => x.BankAccount != null)
                    .Select(x => x.BankAccount)
                    .OrderBy(x => x.BankAccountId).ToArray();
                
                foreach (var bankAccount in bankAccounts)
                {
                    if (bankAccount.Balance >= amount)
                    {
                        bankAccount.Withdraw(amount);
                        amount = 0;
                    }
                    else
                    {
                        amount -= bankAccount.Balance;
                        bankAccount.Withdraw(bankAccount.Balance);
                    }

                    if (amount == 0)
                    {
                        return;
                    }
                }


                var creditCards = user.PaymentMethods
                    .Where(x => x.CreditCard != null)
                    .Select(x => x.CreditCard)
                    .OrderBy(x => x.CreditCardId);

                foreach (var creditCard in creditCards)
                {
                    if (creditCard.LimitLeft >= amount)
                    {
                        creditCard.Withdraw(amount);
                        amount = 0;
                    }
                    else
                    {
                        amount -= creditCard.LimitLeft;
                        creditCard.Withdraw(creditCard.LimitLeft);
                    }

                    if (amount == 0)
                    {
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }
        }

        private static void GetInfo(User user)
        {
            Console.WriteLine($"User: {user.FirstName} {user.LastName}");
            Console.WriteLine("Bank Accounts:");

            var bankAccounts = user.PaymentMethods
                .Where(x => x.BankAccount != null)
                .Select(x => x.BankAccount);
            foreach (var bankAccount in bankAccounts)
            {
                Console.WriteLine($"--ID: {bankAccount.BankAccountId}");
                Console.WriteLine($"--- Balance: {bankAccount.Balance:f2}");
                Console.WriteLine($"--- Bank: {bankAccount.BankName}");
                Console.WriteLine($"--- SWIFT: {bankAccount.SwiftCode}");
            }

            Console.WriteLine("Credit Cards:");

            var creditCards = user.PaymentMethods
                .Where(x => x.CreditCard != null)
                .Select(x => x.CreditCard);

            foreach (var creditCard in creditCards)
            {
                Console.WriteLine($"--ID: {creditCard.CreditCardId}");
                Console.WriteLine($"--- Limit: {creditCard.Limit:f2}");
                Console.WriteLine($"--- Money Owed: {creditCard.MoneyOwed:f2}");
                Console.WriteLine($"--- Limit Left: {creditCard.LimitLeft:f2}");
                Console.WriteLine($"--- Expiration Date: {creditCard.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
            }
        }

        private static User GetUser(BillsPaymentSystemContext context)
        {
            int userId = int.Parse(Console.ReadLine());

            User user = null;

            while (true)
            {
                user = context.Users.Where(x => x.UserId == userId)
                    .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.BankAccount)
                    .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.CreditCard)
                    .FirstOrDefault();

                if (user == null)
                {
                    userId = int.Parse(Console.ReadLine());
                    continue;
                }

                break;
            }

            return user;
        }
    }
}
