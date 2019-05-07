using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Seed
{
    public class PaymentMethodInitializer
    {
        public static PaymentMethod[] GetPaymentMethods()
        {
            PaymentMethod[] paymentMethods = new PaymentMethod[]
            {
                new PaymentMethod() { UserId = 1, BankAccountId = 1, CreditCardId = null, Type = PaymentMethodType.BankAccount},
                new PaymentMethod() { UserId = 3, BankAccountId = 2, CreditCardId = null, Type = PaymentMethodType.BankAccount},
                new PaymentMethod() { UserId = 5, BankAccountId = 3, CreditCardId = null, Type = PaymentMethodType.BankAccount},
                new PaymentMethod() { UserId = 2, BankAccountId = null, CreditCardId = 1, Type = PaymentMethodType.CreditCard},
                new PaymentMethod() { UserId = 4, BankAccountId = null, CreditCardId = 2, Type = PaymentMethodType.CreditCard},
                new PaymentMethod() { UserId = 6, BankAccountId = null, CreditCardId = 3, Type = PaymentMethodType.CreditCard}
            };

            return paymentMethods;
        }
    }
}