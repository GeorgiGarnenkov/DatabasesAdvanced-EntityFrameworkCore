using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfiguration
{
    public class PositionConfig : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.HasKey(p => p.PositionId);

            builder.Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasMany(p => p.Players)
                .WithOne(p => p.Position)
                .HasForeignKey(p => p.PositionId);
        }
    }
}