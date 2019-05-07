using TeamBuilder.App.Utilities;

namespace TeamBuilder.App.Core.Command
{
    public class LogOutCommand : ICommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(0, commandArgs);

            string username = AuthenticationManager.GetCurrentUser()?.Username;

            AuthenticationManager.Logout();

            return string.Format(Constants.SuccessMessages.SuccessfulLogout, username);
        }
    }
}