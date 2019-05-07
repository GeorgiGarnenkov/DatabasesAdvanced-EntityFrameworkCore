using System.Linq;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Utilities
{
    public static class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Any(x => x.Name == teamName);
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Users.Any(x => x.Username == username);
            }
        }

        public static bool IsInviteExisting(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context
                    .Invitations
                    .Any(x => x.Team.Name == teamName && x.InvitedUserId == user.Id && x.IsActive);
            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            if (user.CreatedUserTeams.SingleOrDefault(x => x.Team.Name == teamName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            if (user.CreatedEvents.SingleOrDefault(x => x.Name == eventName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Find(teamName).Members.Any(x => x.User.Username == username);
            }

        }

        public static bool IsEventExisting(string eventName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events.Any(x => x.Name == eventName);
            }
        }

    }
}