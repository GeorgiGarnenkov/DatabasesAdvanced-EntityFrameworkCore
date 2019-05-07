namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Student
    {
        public Student()
        {
            this.CourseEnrollments = new HashSet<StudentCourse>();
            this.HomeworkSubmissions = new HashSet<Homework>();
        }

        public int StudentId { get; set; }

        public string Name { get; set; }            // (up to 100 characters, unicode)
        public string PhoneNumber { get; set; }     // (exactly 10 characters, not unicode, not required)
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }      // (not required)


        public ICollection<StudentCourse> CourseEnrollments { get; set; }
        public ICollection<Homework> HomeworkSubmissions { get; set; }
    }
}