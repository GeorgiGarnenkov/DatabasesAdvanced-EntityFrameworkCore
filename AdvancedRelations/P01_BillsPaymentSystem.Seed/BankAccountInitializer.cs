using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Seed
{
    public class BankAccountInitializer
    {
        public static BankAccount[] GetBankAccounts()
        {
            BankAccount[] bankAccounts = new BankAccount[]
            {
                new BankAccount{BankName = "CCB", SwiftCode = "CECBBGSF"},
                new BankAccount{BankName = "Fibank", SwiftCode = "FINVBGSF"},
                new BankAccount{BankName = "CCB", SwiftCode = "CECBBGSF"},
                new BankAccount{BankName = "OBB", SwiftCode = "UBBSBGSF"},
                new BankAccount{BankName = "TeximBank", SwiftCode = "TEXIBGSF"},
                new BankAccount{BankName = "DSK Bank", SwiftCode = "STSABGSF"}
            };

            return bankAccounts;
        }
    }
}