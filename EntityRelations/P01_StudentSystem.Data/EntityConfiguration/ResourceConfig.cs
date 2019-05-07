using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(r => r.ResourceId);

            builder.Property(p => p.Name)
                   .HasMaxLength(50)
                   .IsRequired()
                   .IsUnicode();

            builder.Property(p => p.Url)
                   .IsUnicode(false);

            builder.HasOne(c => c.Course)
                   .WithMany(r => r.Resources)
                   .HasForeignKey(c => c.CourseId);
        }
    }
}