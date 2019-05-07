using System;
using System.Linq;
using TeamBuilder.App.Core.Command;

namespace TeamBuilder.App.Core
{
    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string result = string.Empty;

            string[] inputArgs = input.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

            string commandName = inputArgs.Length > 0 ? inputArgs[0] : string.Empty;

            inputArgs = inputArgs.Skip(1).ToArray();

            switch (commandName.ToLower())
            {
                case "registeruser":
                    result = new RegisterUserCommand().Execute(inputArgs);
                    break;
                case "login":
                    result = new LogInCommand().Execute(inputArgs);
                    break;
                case "logout":
                    result = new LogOutCommand().Execute(inputArgs);
                    break;
                case "deleteuser":
                    result = new DeleteUserCommand().Execute(inputArgs);
                    break;
                case "createevent":
                    result = new CreateEventCommand().Execute(inputArgs);
                    break;
                case "createteam":
                    result = new CreateTeamCommand().Execute(inputArgs);
                    break;
                case "invitetoteam":
                    result = new InviteToTeamCommand().Execute(inputArgs);
                    break;
                case "acceptinvite":
                    result = new AcceptInviteCommand().Execute(inputArgs);
                    break;
                case "declineinvite":
                    result = new DeclineInviteCommand().Execute(inputArgs);
                    break;
                case "kickmember":
                    result = new KickMemberCommand().Execute(inputArgs);
                    break;
                case "disband":
                    result = new DisbandCommand().Execute(inputArgs);
                    break;
                case "addteamto":
                    result = new AddTeamToCommand().Execute(inputArgs);
                    break;
                case "showevent":
                    result = new ShowEventCommand().Execute(inputArgs);
                    break;
                case "showteam":
                    result = new ShowTeamCommand().Execute(inputArgs);
                    break;


                case "exit":
                    var exit = new ExitCommand().Execute(inputArgs);
                    break;
                default: throw new NotSupportedException($"Command {commandName} not supported!");
            }

            return result;
        }
    }
}