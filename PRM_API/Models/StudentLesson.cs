using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class StudentLesson
    {
        public int Id { get; set; }
        public double Mark { get; set; }
        public string? Note { get; set; }
        public int LessionId { get; set; }
        public int StudentCourseId { get; set; }

        public virtual Lesson Lession { get; set; } = null!;
        public virtual StudentCourse StudentCourse { get; set; } = null!;
    }
}
