using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Course
    {
        public Course()
        {
            StudentCourses = new HashSet<StudentCourse>();
            Units = new HashSet<Unit>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double? RateAverage { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}
