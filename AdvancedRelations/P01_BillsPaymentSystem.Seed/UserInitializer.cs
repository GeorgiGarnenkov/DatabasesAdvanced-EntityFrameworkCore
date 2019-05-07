using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Seed
{
    public class UserInitializer
    {
        public static User[] GetUsers()
        {
            User[] users = new User[]
            {
                new User {FirstName = "Georgi", LastName = "Georgiev", Email = "georgigeorgiev@gmailcom", Password = "georgi123456"},
                new User {FirstName = "Ivan", LastName = "Ivanov", Email = "ivanivanov@abv.bg", Password = "ivan123456" },
                new User {FirstName = "Andrey", LastName = "Andreev", Email = "andreyandreev@abv.bg", Password = "andrey123456" },
                new User {FirstName = "Krasi", LastName = "Krasimirova", Email = "krasikrasimirova@msn.com", Password = "krasi123456" },
                new User {FirstName = "Dimityr", LastName = "Dimitrov", Email = "dimitardimitrov@gmail.com", Password = "dimityr123456" },
                new User {FirstName = "Petyr", LastName = "Petrov", Email = "petyrpetrov@gmail.com", Password = "petyr123456" }
            };

            return users;
        }
    }
}