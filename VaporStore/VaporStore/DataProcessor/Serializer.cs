using System.Linq;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor
{
	using System;
	using Data;

	public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
		    var games = context.Genres
		        .Where(x => genreNames.Any(g => g == x.Name) && x.Games.All(s => s.Purchases.Count > 0))
		        .Select(x => new
		        {
		            Id = x.Id,
		            Genre = x.Name,
		            Games = x.Games.Select(g => new
		                                            {
                                                        Id = g.Id,
                                                        Title = g.Name,
                                                        Developer = g.Developer.Name,
                                                        Tags = g.GameTags,
                                                        Players = g.Purchases.Count
		                                            })
		                                                .OrderByDescending(a => a.Players)
		                                                .ThenBy(w => w.Id)
                                                        .ToArray(),
		            TotalPlayers = x.Games.Sum(s => s.Purchases.Count)
                })
		        .OrderByDescending(x => x.TotalPlayers)
		        .ThenBy(x => x.Id)
		        .ToArray();

		    return JsonConvert.SerializeObject(games, Formatting.Indented);
        }

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			throw new NotImplementedException();
		}
	}
}