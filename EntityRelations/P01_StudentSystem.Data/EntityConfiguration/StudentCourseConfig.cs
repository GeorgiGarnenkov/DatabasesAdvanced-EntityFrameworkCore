using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class StudentCourseConfig : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(x => new { x.StudentId, x.CourseId });

            builder.HasOne(s => s.Student)
                   .WithMany(c => c.CourseEnrollments)
                   .HasForeignKey(s => s.StudentId);

            builder.HasOne(c => c.Course)
                   .WithMany(s => s.StudentsEnrolled)
                   .HasForeignKey(c => c.CourseId);
        }
    }
}