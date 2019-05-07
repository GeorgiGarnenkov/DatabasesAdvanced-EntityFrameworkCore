using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Command
{
    public class ShowTeamCommand : ICommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(1, commandArgs);

            string teamName = commandArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            Team team = null;

            using (var context = new TeamBuilderContext())
            {
                team = context.Teams
                    .Include(t => t.Members)
                    .ThenInclude(m => m.User)
                    .SingleOrDefault(t => t.Name == teamName);
            }

            var sb = new StringBuilder();

            sb.AppendLine($"{team.Name} {team.Acronym}");
            sb.AppendLine("Members:");

            foreach (var m in team.Members)
            {
                sb.AppendLine($"-{m.User.Username}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}