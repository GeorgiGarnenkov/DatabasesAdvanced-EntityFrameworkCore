using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VaporStore.DataProcessor
{
	using System;
	using Data;

	public static class Bonus
	{
		public static string UpdateEmail(VaporStoreDbContext context, string username, string newEmail)
		{
		    var user = context.Users.FirstOrDefault(x => x.Username == username);

		    if (!context.Users.Contains(user))
		    {
		        return $"User {username} not found";
		    }

		    if (context.Users.Any(x => x.Email == newEmail))
		    {
		        return $"Email {newEmail} is already taken";
		    }

		    user.Email = newEmail;

		    return $"Changed {username}'s email successfully";
        }
	}
}
