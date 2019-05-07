using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class HomeworkConfig : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {

            builder.HasKey(k => k.HomeworkId);

            builder.Property(p => p.Content)
                   .IsUnicode(false);

            builder.HasOne(s => s.Student)
                   .WithMany(h => h.HomeworkSubmissions)
                   .HasForeignKey(s => s.StudentId);

            builder.HasOne(c => c.Course)
                   .WithMany(h => h.HomeworkSubmissions)
                   .HasForeignKey(c => c.CourseId);
        }
    }
}