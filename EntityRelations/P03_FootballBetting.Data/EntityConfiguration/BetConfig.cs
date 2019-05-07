using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfiguration
{
    public class BetConfig : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.HasKey(x => x.BetId);

            builder.Property(p => p.Amount)
                .IsRequired();
            builder.Property(p => p.Prediction)
                .IsRequired();
            builder.Property(p => p.DateTime)
                .IsRequired();
        }
    }
}