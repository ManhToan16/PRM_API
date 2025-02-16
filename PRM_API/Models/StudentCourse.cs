using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class StudentCourse
    {
        public StudentCourse()
        {
            StudentLessons = new HashSet<StudentLesson>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public double? Rate { get; set; }
        public string? Comment { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
        public virtual ICollection<StudentLesson> StudentLessons { get; set; }
    }
}
