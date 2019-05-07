using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfiguration
{
    public class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.TeamId);

            builder.HasMany(x => x.HomeGames)
                .WithOne(x => x.HomeTeam)
                .HasForeignKey(x => x.HomeTeamId);

            builder.HasMany(x => x.AwayGames)
                .WithOne(x => x.AwayTeam)
                .HasForeignKey(x => x.AwayTeamId);

            builder.HasMany(x => x.Players)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId);
        }
    }
}