using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.Dto.Import;

namespace VaporStore.DataProcessor
{
    using System;
    using Data;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid Data";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserializedGame = JsonConvert.DeserializeObject<GameDto[]>(jsonString);

            List<Game> games = new List<Game>();

            foreach (GameDto gameDto in deserializedGame)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                if (gameDto.Tags.Length < 1)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!IsValid(gameDto.Developer))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                if (!IsValid(gameDto.Genre))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Developer dev = GetDeveloper(context, gameDto.Developer);
                Genre gen = GetGenre(context, gameDto.Genre);

                List<GameTag> tags = new List<GameTag>();
                foreach (string gameDtoTag in gameDto.Tags)
                {
                    var tag = GetTag(context, gameDtoTag);

                    tags.Add(tag);
                }


                Game game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = dev,
                    Genre = gen,
                    GameTags = tags

                };

                games.Add(game);

            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
            
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserializedUser = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            List<User> users = new List<User>();

            foreach (UserDto userDto in deserializedUser)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var breaking = false;
                List<Card> cards = new List<Card>();
                foreach (CardDto cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        breaking = true;
                        break;
                    }

                    var card = new Card();
                    card.Number = cardDto.Number;
                    card.Cvc = cardDto.Cvc;
                    card.Type = Enum.Parse<CardType>(cardDto.Type);

                    cards.Add(card);
                }
                if (breaking)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                User user = new User();

                user.FullName = userDto.FullName;
                user.Username = userDto.Username;
                user.Email = userDto.Email;
                user.Age = userDto.Age;
                if (cards.Count < 1)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                user.Cards = cards;
                

                users.Add(user);

            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string devName)
        {
            var developer = context.Developers.FirstOrDefault(x => x.Name == devName);

            if (developer == null)
            {
                developer = new Developer()
                {
                    Name = devName
                };

                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }
        private static Genre GetGenre(VaporStoreDbContext context, string genreName)
        {
            var genre = context.Genres.FirstOrDefault(x => x.Name == genreName);

            if (genre == null)
            {
                genre = new Genre()
                {
                    Name = genreName
                };

                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }
        private static GameTag GetTag(VaporStoreDbContext context, string tagName)
        {
            GameTag tag = context.GameTags.FirstOrDefault(x => x.Tag.Name == tagName);

            if (tag == null || tag.Tag.GameTags.Any(x => x.Tag.Name != tagName))
            {
                if (tag != null)
                {
                    tag.Tag.Name = tagName;

                    context.GameTags.Add(tag);
                }

                context.SaveChanges();
            }

            return tag;
        }
    }
}