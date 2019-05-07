using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamBuilder.Models;

namespace TeamBuilder.Data.EntityConfig
{
    public class EventTeamConfig : IEntityTypeConfiguration<EventTeam>
    {
        public void Configure(EntityTypeBuilder<EventTeam> builder)
        {
            builder.HasKey(x => new { x.EventId, x.TeamId });
            
            builder.HasOne(x => x.Event)
                .WithMany(x => x.ParticipatingEventTeams)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Team)
                .WithMany(x => x.EventsParticipated)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}