using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId);

            builder.Property(p => p.Name)
                   .HasMaxLength(100)
                   .IsRequired()
                   .IsUnicode();

            builder.Property(p => p.PhoneNumber)
                   .HasMaxLength(10)
                   .IsUnicode(false)
                   .IsRequired(false);

            builder.Property(s => s.RegisteredOn)
                   .HasColumnType("DATETIME2");

            builder.Property(s => s.Birthday)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);
        }
    }
}