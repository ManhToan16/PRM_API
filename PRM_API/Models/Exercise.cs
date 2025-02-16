using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Exercise
    {
        public Exercise()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public int Status { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int TypeId { get; set; }
        public int LessonId { get; set; }
        public string? File { get; set; }
        public string? FileName { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
        public virtual Setting Type { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; }
    }
}
