using System;
using System.Linq;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Command
{
    public class InviteToTeamCommand : ICommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(2, commandArgs);
            AuthenticationManager.Authorize();

            string teamName = commandArgs[0];
            string username = commandArgs[1];

            var loggedUser = AuthenticationManager.GetCurrentUser();
            var team = GetTeam(teamName);
            var invitedUser = GetUser(username);

            if (invitedUser == loggedUser)
            {
                AddCreatorToGroup(team);

                return string.Format(Constants.SuccessMessages.SuccessfullySentInvitation, teamName, username);
            }

            CheckInputParams(team, invitedUser);

            CheckIfUsersBelongToGroup(team, loggedUser, invitedUser);

            CheckAlreadyActiveInvite(team, invitedUser);

            AddInviteToDb(team, invitedUser);

            return string.Format(Constants.SuccessMessages.SuccessfullySentInvitation, teamName, username);
        }

        private void AddInviteToDb(Team team, User invitedUser)
        {
            using (var context = new TeamBuilderContext())
            {
                context.Invitations.Add(new Invitation
                {
                    InvitedUserId = invitedUser.Id,
                    TeamId = team.Id
                });

                context.SaveChanges();
            }
        }

        private void AddCreatorToGroup(Team team)
        {
            using (var context = new TeamBuilderContext())
            {
                context.UserTeams.Add(new UserTeam
                {
                    UserId = team.CreatorId,
                    TeamId = team.Id
                });

                context.SaveChanges();
            }
        }

        private void CheckAlreadyActiveInvite(Team team, User invitedUser)
        {
            if (CommandHelper.IsInviteExisting(team.Name, invitedUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }
        }

        private void CheckIfUsersBelongToGroup(Team team, User logged, User invited)
        {
            using (var context = new TeamBuilderContext())
            {
                if (!CommandHelper.IsUserCreatorOfTeam(team.Name, logged) ||
                    CommandHelper.IsMemberOfTeam(team.Name, logged.Username) ||
                    CommandHelper.IsMemberOfTeam(team.Name, invited.Username))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
                }
            }
        }

        private static void CheckInputParams(Team team, User invitedUser)
        {
            if (team == null || invitedUser == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }
        }

        private User GetUser(string username)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Users.FirstOrDefault(u => u.Username == username);
            }
        }

        private Team GetTeam(string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.FirstOrDefault(t => t.Name == teamName);
            }
        }
    }
}