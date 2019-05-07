using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.CourseId);

            builder.Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            builder.Property(p => p.Description)
                .IsUnicode()
                .IsRequired(false);

            builder.Property(p => p.StartDate)
                .HasColumnType("DATETIME2");

            builder.Property(p => p.EndDate)
                .HasColumnType("DATETIME2");
        }
    }
}