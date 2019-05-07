using System.Linq;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;

namespace TeamBuilder.App.Core.Command
{
    public class DeleteUserCommand : ICommand
    {
        public string Execute(string[] commandParams)
        {
            Check.CheckLength(0, commandParams);
            AuthenticationManager.Authorize();

            var user = AuthenticationManager.GetCurrentUser();
            var username = user.Username;

            using (var context = new TeamBuilderContext())
            {
                context.Users.FirstOrDefault(u => u.Id == user.Id).IsDeleted = true;
                context.SaveChanges();
            }

            AuthenticationManager.Logout();

            return string.Format(Constants.SuccessMessages.SuccessfullyDeletedUser, username);
        }
    }
}