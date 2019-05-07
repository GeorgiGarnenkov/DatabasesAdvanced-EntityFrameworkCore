using System;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;
using TeamBuilder.Models.Enums;

namespace TeamBuilder.App.Core.Command
{
    public class RegisterUserCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(7, args);

            string userName = args[0];
            string password = args[1];
            string repeatPassword = args[2];
            string firstName = args[3];
            string lastName = args[4];
            int age;
            Gender gender;

            if (userName.Length < Constants.MinUsernameLength || userName.Length > Constants.MaxUsernameLength )
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UsernameNotValid, userName));
            }

            if (password.Length < Constants.MinPasswordLength || password.Length > Constants.MaxPasswordLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.PasswordNotValid, password));
            }

            if (!int.TryParse(args[5], out age))
            {
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            }

            if (!Enum.TryParse<Gender>(args[6], out gender))
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            }

            if (password != repeatPassword)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.PasswordDoesNotMatch);
            }

            if (CommandHelper.IsUserExisting(userName))
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsTaken, userName));
            }

            User user = new User()
            {
                Username = userName,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Gender = gender
            };

            using (var context = new TeamBuilderContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.SuccessfullyRegisteredUser, userName);
        }
    }
}