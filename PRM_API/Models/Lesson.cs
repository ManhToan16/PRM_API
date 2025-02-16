using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Exercises = new HashSet<Exercise>();
            StudentLessons = new HashSet<StudentLesson>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UnitId { get; set; }
        public string Description { get; set; } = null!;
        public string? Note { get; set; }
        public int Status { get; set; }

        public virtual Unit Unit { get; set; } = null!;
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<StudentLesson> StudentLessons { get; set; }
    }
}
