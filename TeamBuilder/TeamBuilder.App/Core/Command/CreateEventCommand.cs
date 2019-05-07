using System;
using System.Globalization;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Command
{
    public class CreateEventCommand : ICommand
    {
        public string Execute(string[] commandArgs)
        {
            Check.CheckLength(6, commandArgs);
            AuthenticationManager.Authorize();

            var user = AuthenticationManager.GetCurrentUser();

            var eventName = commandArgs[0];
            CheckEventName(eventName);

            var eventDescription = commandArgs[1];
            CheckEventDescription(eventDescription);

            string fullStartDate = $"{commandArgs[2]} {commandArgs[3]}";
            DateTime startDate;
            bool isStartDateValid = DateTime
                .TryParseExact(fullStartDate, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            string fullEndDate = $"{commandArgs[4]} {commandArgs[5]}";
            DateTime endDate;
            bool isEndDateValid = DateTime
                .TryParseExact(fullEndDate, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

            if (!isStartDateValid || !isEndDateValid)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            AddEventToDb(user, eventName, eventDescription, startDate, endDate);

            return string.Format(Constants.SuccessMessages.SuccessfullyCreatedEvent, eventName);
        }

        private static void AddEventToDb(
            User user, string eventName, string eventDescription, DateTime startDate, DateTime endDate)
        {
            using (var context = new TeamBuilderContext())
            {
                var newEvent = new Event
                {
                    Name = eventName,
                    Description = eventDescription,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatorId = user.Id
                };

                context.Events.Add(newEvent);
                context.SaveChanges();
            }
        }

        private void CheckEventName(string eventName)
        {
            if (eventName.Length > Constants.MaxEventNameLength)
            {
                throw new ArgumentException("Event name should be up to 25 characters long!");
            }
        }

        private void CheckEventDescription(string eventDescription)
        {
            if (eventDescription.Length > Constants.MaxEventDescriptionLength)
            {
                throw new ArgumentException("Event description should be up to 250 characters long!");
            }
        }
    }
}